using AdsReportingPortal.Api.Service.Interface;
using AdsReportingPortal.Data.Repository.Interface;
using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using RestSharp;

namespace AdsReportingPortal.Api.Service.Implementation
{
    public class JobService : IJob
    {
        private readonly IAdsPortalRepo<Campaigns> _campaignRepo;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ILogger<JobService> _logger;

        private readonly IAdsStatService _statService;

        public JobService(IAdsPortalRepo<Campaigns> campaignRepo,
            IAccessTokenService accessTokenService,
            ILogger<JobService> logger,

            IAdsStatService statService)
        {
            _campaignRepo = campaignRepo;
            _accessTokenService = accessTokenService;
            _logger = logger;

            _statService = statService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var campaigns = await _campaignRepo.GetQueryable().ToListAsync();
            if (!campaigns.Any())
            {
                _logger.LogError("No campaign IDs available.");
                return;
            }

            foreach (var campaign in campaigns)
            {
                var metaGenderUrl = await _accessTokenService.ConstructUrlMetaGender(campaign.CampaignId);
                if (metaGenderUrl.StatusCode != 200)
                {
                    LogError(metaGenderUrl);
                    continue;
                }

                var publisherUrl = await _accessTokenService.ConstructUrlPublisher(campaign.CampaignId);
                if (publisherUrl.StatusCode != 200)
                {
                    LogError(publisherUrl);
                    continue;
                }

                try
                {
                    var metaGenResponse = await FetchApiResponse<MetaGenRootApi>(metaGenderUrl.Result);
                    if (!metaGenResponse.IsSuccessStatusCode)
                    {
                        LogApiError("MetaGender", metaGenResponse);
                        continue;
                    }

                    var publisherResponse = await FetchApiResponse<PublisherApiResponse>(publisherUrl.Result);
                    if (!publisherResponse.IsSuccessStatusCode)
                    {
                        LogApiError("Publisher", publisherResponse);
                        continue;
                    }

                    var metaData = JsonConvert.DeserializeObject<MetaGenRootApi>(metaGenResponse.Content);
                    var publisherData = JsonConvert.DeserializeObject<PublisherApiResponse>(publisherResponse.Content);

                    UpdateMetaDataWithPublisherPlatform(metaData, publisherData);

                    var saveStats = await _statService.AddStatsMetaGenderAge(metaData.Data);
                    if (saveStats.StatusCode != 200)
                    {
                        _logger.LogError(saveStats.DisplayMessage, saveStats.ErrorMessages);
                        continue;
                    }

                    _logger.LogInformation("Successfully saved stats for campaign {CampaignId}", campaign.CampaignId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching data for campaign {CampaignId}", campaign.CampaignId);
                }
            }
        }

        private async Task<RestResponse<T>> FetchApiResponse<T>(string url) where T : new()
        {
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            return await client.ExecuteAsync<T>(request);
        }

        private void UpdateMetaDataWithPublisherPlatform(MetaGenRootApi metaData, PublisherApiResponse publisherData)
        {
            foreach (var item in metaData.Data)
            {
                item.platform = publisherData.data[0].publisher_platform;
            }
        }

        private void LogError(ResponseDto<string> urlResponse)
        {
            _logger.LogError(urlResponse.DisplayMessage, urlResponse.ErrorMessages);
        }

        private void LogApiError(string apiName, RestResponse response)
        {
            _logger.LogError("Error from {ApiName} API: {Content}", apiName, response.Content);
            _logger.LogError("Error from {ApiName} API: {ErrorMessage}", apiName, response.ErrorMessage);
            _logger.LogError("Error from {ApiName} API: {ErrorException}", apiName, response.ErrorException);
        }

    }
}

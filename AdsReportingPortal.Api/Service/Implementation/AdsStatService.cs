using AdsReportingPortal.Api.Service.Interface;
using AdsReportingPortal.Data.Repository.Interface;
using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace AdsReportingPortal.Api.Service.Implementation
{
    public class AdsStatService : IAdsStatService
    {
        private readonly ILogger<AccessTokenService> _logger;
        private readonly IAdsPortalRepo<CampaignStats> _campaignStatRepo;
        private readonly IAdsPortalRepo<MetaAgeGenderCategory> _metaAgeGenderRepo;
        private readonly IAdsPortalRepo<CostPerActionType> _costPerActionTypeRepo;
        private readonly IAdsPortalRepo<MetaAction> _actionRepo;
        private readonly IAdsPortalRepo<VideoPlayAction> _videoPlayRepo;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        public AdsStatService(IConfiguration configuration,
            IEncryptionService encryptionService,
            ILogger<AccessTokenService> logger,
            IAdsPortalRepo<CampaignStats> campaignStatRepo,
            IAdsPortalRepo<MetaAgeGenderCategory> metaAgeGenderRepo,
            IAdsPortalRepo<CostPerActionType> costPerActionTypeRepo,
            IAdsPortalRepo<MetaAction> actionRepo,
            IAdsPortalRepo<VideoPlayAction> videoPlayRepo,
            IAccessTokenService accessTokenService)
        {
            _configuration = configuration;
            _encryptionService = encryptionService;
            _logger = logger;
            _campaignStatRepo = campaignStatRepo;
            _metaAgeGenderRepo = metaAgeGenderRepo;
            _costPerActionTypeRepo = costPerActionTypeRepo;
            _actionRepo = actionRepo;
            _videoPlayRepo = videoPlayRepo;
            _accessTokenService = accessTokenService;
        }

        public async Task<ResponseDto<GetCampaignsResponse>> GetCampaignsAsync(string ads_id)
        {
            var response = new ResponseDto<GetCampaignsResponse>();
            try
            {
                var getAccessToken = await _accessTokenService.GetToken();
                if (getAccessToken.StatusCode != 200)
                {
                    response.StatusCode = getAccessToken.StatusCode;
                    response.ErrorMessages = getAccessToken.ErrorMessages;
                    response.DisplayMessage = getAccessToken.DisplayMessage;
                    return response;
                }

                var decrptToken = _encryptionService.Decrypt(getAccessToken.Result, _configuration["Key:enkey"]);

                var client = new RestClient("https://graph.facebook.com/v17.0/");
                // Define the request
                var request = new RestRequest($"{ads_id}/campaigns", Method.Get);

                // Add query parameters
                request.AddQueryParameter("fields", "id,name,status,objective,start_time,stop_time,budget_remaining,daily_budget");
                request.AddQueryParameter("access_token", decrptToken);

                // Execute the request
                var resp = await client.ExecuteAsync(request);

                // Ensure the request was successful
                if (!resp.IsSuccessful)
                {
                    response.StatusCode = 400;
                    response.ErrorMessages = new List<string>() { $"{resp.ErrorMessage}" };
                    response.DisplayMessage = "Error";
                    return response;
                }

                var campaignsResponse = JsonConvert.DeserializeObject<GetCampaignsResponse>(resp.Content);
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                response.Result = campaignsResponse;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string>() { "Unable to retrieve campaigns successfully" };
                return response;
            }
        }

        public async Task<ResponseDto<GetAdsAccountResponse>> GetAdAccountsAsync()
        {
            var response = new ResponseDto<GetAdsAccountResponse>();
            try
            {

                var getAccessToken = await _accessTokenService.GetToken();
                if (getAccessToken.StatusCode != 200)
                {
                    response.StatusCode = getAccessToken.StatusCode;
                    response.ErrorMessages = getAccessToken.ErrorMessages;
                    response.DisplayMessage = getAccessToken.DisplayMessage;
                    return response;
                }

                var decrptToken = _encryptionService.Decrypt(getAccessToken.Result, _configuration["Key:enkey"]);

                var client = new RestClient("https://graph.facebook.com/v17.0/");
                // Define the request
                var request = new RestRequest("me/adaccounts", Method.Get);

                // Add query parameters
                request.AddQueryParameter("fields", "id,name,account_status,currency,amount_spent,business,created_time,daily_spend_limit,timezone_name,owner");
                request.AddQueryParameter("access_token", decrptToken);

                // Execute the request
                var resp = await client.ExecuteAsync(request);

                // Ensure the request was successful
                if (!resp.IsSuccessful)
                {

                    response.StatusCode = 400;
                    response.ErrorMessages = new List<string>() { $"{resp.ErrorMessage}" };
                    response.DisplayMessage = "Error";
                    return response;


                }
                var adAccountsResponse = JsonConvert.DeserializeObject<GetAdsAccountResponse>(resp.Content);
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                response.Result = adAccountsResponse;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.DisplayMessage = "error";
                response.ErrorMessages = new List<string>() { "Unable to get Ads account from meta succcessfully not added successfully" };
                return response;
            }
        }


        public async Task<ResponseDto<string>> AddStatsMetaGenderAge(List<MetaGenApiResponse> ads)
        {
            var response = new ResponseDto<string>();
            try
            {
                var adsLatest = new List<MetaGenApiResponse>();
                adsLatest = ads;
                foreach (var adsItem in adsLatest)
                {
                    var addMeta = await _metaAgeGenderRepo.Add(
                        new MetaAgeGenderCategory()
                        {
                            age = adsItem.age,
                            clicks = adsItem.clicks,
                            cpm = adsItem.cpm,
                            ctr = adsItem.ctr,
                            date_start = adsItem.date_start,
                            date_stop = adsItem.date_stop,
                            frequency = adsItem.frequency,
                            impressions = adsItem.impressions,
                            reach = adsItem.reach,
                            gender = adsItem.gender,
                            spend = adsItem.spend,
                            unique_clicks = adsItem.unique_clicks,
                        }
                       );


                }
                response.StatusCode = 200;
                response.DisplayMessage = "success";
                response.Result = "Campaign Stats Added";
                await _metaAgeGenderRepo.SaveChanges();
                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.DisplayMessage = "error";
                response.ErrorMessages = new List<string>() { "Campaign not added successfully" };
                return response;
            }
        }
        public async Task<ResponseDto<string>> AddStats(string CampaingnId, string impressionCount, string spend)
        {
            var response = new ResponseDto<string>();
            try
            {
                await _campaignStatRepo.Add(new CampaignStats() { CampaignsId = CampaingnId, Impression = impressionCount, Budget = spend });
                await _campaignStatRepo.SaveChanges();
                response.StatusCode = 200;
                response.DisplayMessage = "success";
                response.Result = "Campaign Stats Added";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.DisplayMessage = "error";
                response.ErrorMessages = new List<string>() { "Campaign not added successfully" };
                return response;
            }
        }
        public async Task<ResponseDto<IEnumerable<ImpressionsByHour>>> GetImpressionAggregate(DateTime dateTime)
        {
            var response = new ResponseDto<IEnumerable<ImpressionsByHour>>();
            try
            {
                var impressionMetricsRaw = await _metaAgeGenderRepo.GetQueryable()
                    .Where(u => u.Created.Date == dateTime.Date)
                    .Select(m => new
                    {
                        m.Created,
                        m.impressions
                    })
                    .ToListAsync();

                var impressionMetrics = impressionMetricsRaw
                    .GroupBy(m => new DateTime(m.Created.Year, m.Created.Month, m.Created.Day, m.Created.Hour, 0, 0))
                    .Select(g => new ImpressionsByHour
                    {
                        Hour = g.Key.Hour,
                        TotalImpressions = g.Sum(m => long.Parse(m.impressions))
                    })
                    .OrderBy(r => r.Hour)
                    .ToList();

                response.StatusCode = 200;
                response.DisplayMessage = "success";
                response.Result = impressionMetrics;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.DisplayMessage = "error";
                response.ErrorMessages = new List<string>() { "Failed to aggregate impressions" };
                return response;
            }
        }

        public async Task<ResponseDto<PaginatedAdsStats>> GetStatsPaginatedAggregate(DateTime dateTime, int pageNumber, int perPageSize)
        {
            var response = new ResponseDto<PaginatedAdsStats>();
            try
            {
                pageNumber = pageNumber < 1 ? 1 : pageNumber;
                perPageSize = perPageSize < 1 ? 5 : perPageSize;
                var impressionMetricsRaw = _metaAgeGenderRepo.GetQueryable()
                    .Where(u => u.Created.Date == dateTime.Date)
                    .Select(m => new MetaAdsAllDailyResponse
                    {
                        age = m.age,
                        clicks = m.clicks,
                        cpm = m.cpm,
                        ctr = m.ctr,
                        date_start = m.date_start,
                        frequency = m.frequency,
                        date_stop = m.date_stop,
                        gender = m.gender,
                        impressions = m.impressions,

                        reach = m.reach,
                        spend = m.spend,
                        stat_date = m.Created,

                        unique_clicks = m.unique_clicks
                    });

                var totalCount = await impressionMetricsRaw.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / perPageSize);
                var paginatedMetrics = await impressionMetricsRaw
               .Skip((pageNumber - 1) * perPageSize)
               .Take(perPageSize)
               .ToListAsync();
                var result = new PaginatedAdsStats()
                {
                    CurrentPage = pageNumber,
                    PageSize = perPageSize,
                    TotalPages = totalPages,
                    MetaStats = paginatedMetrics
                };
                response.StatusCode = 200;
                response.DisplayMessage = "success";
                response.Result = result;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.DisplayMessage = "error";
                response.ErrorMessages = new List<string>() { "Failed paginate" };
                return response;
            }
        }

        public async Task<ResponseDto<IEnumerable<dynamic>>> GetOthersAggregate(DateTime dateTime, int aggregateType)
        {
            var response = new ResponseDto<IEnumerable<dynamic>>();
            try
            {


                var MetricsRaw = await _metaAgeGenderRepo.GetQueryable()
                    .Where(u => u.Created.Date == dateTime.Date)
                    .Select(m => new
                    {
                        m.Created,
                        m.reach,
                        m.clicks,
                        m.spend,
                        m.cpm
                    })
                    .ToListAsync();


                if (aggregateType == 1)
                {
                    var totalMetrics = MetricsRaw
                         .GroupBy(m => new DateTime(m.Created.Year, m.Created.Month, m.Created.Day, m.Created.Hour, 0, 0))
                         .Select(g => new ReachByHour
                         {
                             Hour = g.Key.Hour,
                             TotalReach = g.Sum(m => long.Parse(m.reach))
                         })
                         .OrderBy(r => r.Hour)
                         .ToList();
                    response.StatusCode = 200;
                    response.DisplayMessage = "success";
                    response.Result = totalMetrics;
                    return response;

                }
                else if (aggregateType == 2)
                {
                    var totalMetrics = MetricsRaw
                     .GroupBy(m => new DateTime(m.Created.Year, m.Created.Month, m.Created.Day, m.Created.Hour, 0, 0))
                     .Select(g => new ClickByHours
                     {
                         Hour = g.Key.Hour,
                         TotalClicks = g.Sum(m => long.Parse(m.reach))
                     })
                     .OrderBy(r => r.Hour)
                     .ToList();
                    response.StatusCode = 200;
                    response.DisplayMessage = "success";
                    response.Result = totalMetrics;
                    return response;
                }
                else if (aggregateType == 3)
                {
                    var totalMetrics = MetricsRaw
                        .GroupBy(m => new DateTime(m.Created.Year, m.Created.Month, m.Created.Day, m.Created.Hour, 0, 0))
                        .Select(g => new SpendByHour
                        {
                            Hour = g.Key.Hour,
                            TotalSpend = g.Sum(m => decimal.Parse(m.spend))
                        })
                        .OrderBy(r => r.Hour)
                        .ToList();
                    response.StatusCode = 200;
                    response.DisplayMessage = "success";
                    response.Result = totalMetrics;
                    return response;
                }
                else
                {
                    var totalMetrics = MetricsRaw
                     .GroupBy(m => new DateTime(m.Created.Year, m.Created.Month, m.Created.Day, m.Created.Hour, 0, 0))
                     .Select(g => new CpmByHour
                     {
                         Hour = g.Key.Hour,
                         TotalCpm = g.Sum(m => decimal.Parse(m.cpm))
                     })
                     .OrderBy(r => r.Hour)
                     .ToList();
                    response.StatusCode = 200;
                    response.DisplayMessage = "success";
                    response.Result = totalMetrics;
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.DisplayMessage = "error";
                response.ErrorMessages = new List<string>() { "Failed to aggregate reach" };
                return response;
            }
        }

    }

}

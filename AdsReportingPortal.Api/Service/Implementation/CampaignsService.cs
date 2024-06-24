using AdsReportingPortal.Api.Service.Interface;
using AdsReportingPortal.Data.Repository.Interface;
using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdsReportingPortal.Api.Service.Implementation
{
    public class CampaignsService : ICampaignsService
    {
        private readonly IAdsPortalRepo<Campaigns> _campaignRepo;
        private readonly ILogger<CampaignsService> _logger;

        public CampaignsService(IAdsPortalRepo<Campaigns> campaignRepo, ILogger<CampaignsService> logger)
        {
            _campaignRepo = campaignRepo;
            _logger = logger;
        }

        public async Task<ResponseDto<string>> AddCampaign(string id, string name)
        {
            var response = new ResponseDto<string>();
            try
            {
                var checkcampaigns = await _campaignRepo.GetQueryable().FirstOrDefaultAsync(u=>u.CampaignId == id);
                if (checkcampaigns != null)
                {

                   response.StatusCode = 400;
                    response.ErrorMessages = new List<string>() { "Campaign id already exist"};
                    response.DisplayMessage = "error";
                    return response;

                }
               
                await _campaignRepo.Add(new Campaigns() { CampaignId= id, CampaignName = name });
                await _campaignRepo.SaveChanges();
                response.StatusCode = 200;
                response.Result = "Save campaign id successfully";
                response.DisplayMessage = "success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in creating access token" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;

            }
        }
    }
}

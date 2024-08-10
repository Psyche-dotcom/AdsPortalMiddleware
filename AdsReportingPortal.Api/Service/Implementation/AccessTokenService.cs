using AdsReportingPortal.Api.Service.Interface;
using AdsReportingPortal.Data.Repository.Interface;
using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdsReportingPortal.Api.Service.Implementation
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ILogger<AccessTokenService> _logger;
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly IAdsPortalRepo<AccessToken> _accessTokenRepo;

        public AccessTokenService(ILogger<AccessTokenService> logger,
            IEncryptionService encryptionService,
            IAdsPortalRepo<AccessToken> accessTokenRepo,
            IConfiguration configuration)
        {
            _logger = logger;
            _encryptionService = encryptionService;
            _accessTokenRepo = accessTokenRepo;
            _configuration = configuration;
        }

        public async Task<ResponseDto<string>> AddToken(string Token)
        {
            var response = new ResponseDto<string>();
            try
            {
                var checkToken = await _accessTokenRepo.GetQueryable().FirstOrDefaultAsync();
                if (checkToken != null)
                {

                    _accessTokenRepo.Delete(checkToken);

                }
                var encryptToken = _encryptionService.Encrypt(Token, _configuration["Key:enkey"]);
                await _accessTokenRepo.Add(new AccessToken() { Token = encryptToken });
                await _accessTokenRepo.SaveChanges();
                response.StatusCode = 200;
                response.Result = "Save access token successfully";
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
        public async Task<ResponseDto<string>> ConstructUrl(string campaignid)
        {
            var response = new ResponseDto<string>();
            try
            {
                var getAccessToken = await GetToken();
                if (getAccessToken.StatusCode != 200)
                {
                    response.StatusCode = getAccessToken.StatusCode;
                    response.ErrorMessages = getAccessToken.ErrorMessages;
                    response.DisplayMessage = getAccessToken.DisplayMessage;
                    return response;
                }
                var decrptToken = _encryptionService.Decrypt(getAccessToken.Result,
                    _configuration["Key:enkey"]);
                string url = $"https://graph.facebook.com/v20.0/{campaignid}/insights?date_preset=this_year&access_token={decrptToken}";
                response.StatusCode = 200;
                response.DisplayMessage = "success";
                response.Result = url;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in generating url" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> ConstructUrlMetaGender(string campaignid, 
            bool isDateRange = false,
            string? startDate = null,
    string? endDate = null)
        {

            var response = new ResponseDto<string>();
            try
            {
                var getAccessToken = await GetToken();
                if (getAccessToken.StatusCode != 200)
                {
                    response.StatusCode = getAccessToken.StatusCode;
                    response.ErrorMessages = getAccessToken.ErrorMessages;
                    response.DisplayMessage = getAccessToken.DisplayMessage;
                    return response;
                }

                var decrptToken = _encryptionService.Decrypt(getAccessToken.Result, _configuration["Key:enkey"]);

                // Get the number of months to subtract from app settings
                int monthsToSubtract = int.TryParse(_configuration["Ads:MonthsToSubtract"], out var result) ? result : 0;

                // Calculate the since and until dates
                DateTime currentDate = DateTime.UtcNow.Date;
                DateTime untilDate = currentDate.AddMonths(-monthsToSubtract);
                DateTime sinceDate = currentDate.AddMonths(-monthsToSubtract);



                // Format dates to "yyyy-MM-dd"
                string since;
                string until;
                if (isDateRange)
                {
                    since = startDate;
                    until = endDate;
                }
                else
                {
                     since = sinceDate.ToString("yyyy-MM-dd");
                     until = untilDate.ToString("yyyy-MM-dd");
                }
              

                string otherfield = "fields=cpm,impressions,reach,clicks,ctr,spend,frequency,unique_clicks,conversion_rate_ranking,quality_ranking&";
                string breakdown = "breakdowns=age,gender&";

                // Construct the URL with dynamic time range
                string url = $"https://graph.facebook.com/v20.0/{campaignid}/insights?{otherfield}{breakdown}time_range={{\"since\":\"{since}\",\"until\":\"{until}\"}}&time_increment=all_days&access_token={decrptToken}";

                response.StatusCode = 200;
                response.DisplayMessage = "success";
                response.Result = url;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in generating url" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> ConstructUrlPublisher(string campaignid,
      bool isDateRange = false,
      string? startDate = null,
      string? endDate = null)
        {
            var response = new ResponseDto<string>();
            try
            {
                var getAccessToken = await GetToken();
                if (getAccessToken.StatusCode != 200)
                {
                    response.StatusCode = getAccessToken.StatusCode;
                    response.ErrorMessages = getAccessToken.ErrorMessages;
                    response.DisplayMessage = getAccessToken.DisplayMessage;
                    return response;
                }

                var decrptToken = _encryptionService.Decrypt(getAccessToken.Result, _configuration["Key:enkey"]);

                // Get the number of months to subtract from app settings
                int monthsToSubtract = int.TryParse(_configuration["Ads:MonthsToSubtract"], out var result) ? result : 0;

                // Calculate the since and until dates
                DateTime currentDate = DateTime.UtcNow.Date;
                DateTime untilDate = currentDate.AddMonths(-monthsToSubtract);
                DateTime sinceDate = currentDate.AddMonths(-monthsToSubtract);

                // Format dates to "yyyy-MM-dd"
                string since;
                string until;

                if (isDateRange && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    since = startDate;
                    until = endDate;
                }
                else
                {
                    since = sinceDate.ToString("yyyy-MM-dd");
                    until = untilDate.ToString("yyyy-MM-dd");
                }

                string breakdown = "breakdowns=publisher_platform&";

                // Construct the URL with dynamic time range
                string url = $"https://graph.facebook.com/v20.0/{campaignid}/insights?{breakdown}time_range={{\"since\":\"{since}\",\"until\":\"{until}\"}}&access_token={decrptToken}";

                response.StatusCode = 200;
                response.DisplayMessage = "success";
                response.Result = url;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in generating url" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> GetToken()
        {
            var result = new ResponseDto<string>();
            try
            {
                var getToken = await _accessTokenRepo.GetQueryable().FirstOrDefaultAsync();
                if (getToken == null)
                {

                    result.ErrorMessages = new List<string>() { "Please add a access token" };
                    result.StatusCode = 400;
                    result.DisplayMessage = "Error";
                    return result;
                }
                result.StatusCode = 200;
                result.DisplayMessage = "success";
                result.Result = getToken.Token;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                result.ErrorMessages = new List<string>() { "Error in getting access token" };
                result.StatusCode = 500;
                result.DisplayMessage = "Error";
                return result;
            }
        }
    }
}

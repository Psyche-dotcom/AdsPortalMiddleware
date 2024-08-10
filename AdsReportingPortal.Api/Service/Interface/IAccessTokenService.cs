using AdsReportingPortal.Model.DTO;

namespace AdsReportingPortal.Api.Service.Interface
{
    public interface IAccessTokenService
    {
        Task<ResponseDto<string>> AddToken(string Token);
        Task<ResponseDto<string>> GetToken();
        Task<ResponseDto<string>> ConstructUrlMetaGender(string campaignid,
            bool isDateRange = false,
            string? startDate = null,
    string? endDate = null);
        Task<ResponseDto<string>> ConstructUrl(string campaignid);
        Task<ResponseDto<string>> ConstructUrlPublisher(string campaignid,
         bool isDateRange = false,
         string? startDate = null,
         string? endDate = null);
    }
}

using AdsReportingPortal.Model.DTO;

namespace AdsReportingPortal.Api.Service.Interface
{
    public interface IAccessTokenService
    {
        Task<ResponseDto<string>> AddToken(string Token);
        Task<ResponseDto<string>> GetToken();
        Task<ResponseDto<string>> ConstructUrlPublisher(string campaignid);
        Task<ResponseDto<string>> ConstructUrl(string campaignid);
        Task<ResponseDto<string>> ConstructUrlMetaGender(string campaignid);
    }
}

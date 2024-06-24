using AdsReportingPortal.Model.DTO;

namespace AdsReportingPortal.Api.Service.Interface
{
    public interface ICampaignsService
    {
        Task<ResponseDto<string>> AddCampaign(string id, string name);
    }
}

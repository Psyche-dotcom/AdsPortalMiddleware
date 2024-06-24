

using AdsReportingPortal.Model.Entities;

namespace AdsReportingPortal.Service.Interface
{
    public interface IGenerateJwt
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}

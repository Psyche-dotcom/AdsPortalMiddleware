

using AdsReportingPortal.Model.DTO;

namespace AdsReportingPortal.Service.Interface
{
    public interface IEmailServices
    {
        void SendEmail(Message message);
    }
}

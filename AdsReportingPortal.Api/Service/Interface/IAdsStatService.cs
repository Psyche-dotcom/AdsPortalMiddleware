using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;

namespace AdsReportingPortal.Api.Service.Interface
{
    public interface IAdsStatService
    {
        Task<ResponseDto<string>> AddStats(string CampaingnId, string impressionCount, string spend);
        Task<ResponseDto<PaginatedAdsStats>> GetStatsPaginatedAggregate(DateTime dateTime, int pageNumber, int perPageSize);
        Task<ResponseDto<string>> AddStatsMetaGenderAge(List<MetaGenApiResponse> ads);
        Task<ResponseDto<IEnumerable<ImpressionsByHour>>> GetImpressionAggregate(DateTime dateTime);

        Task<ResponseDto<IEnumerable<dynamic>>> GetOthersAggregate(DateTime dateTime, int aggregateType);
    }
}

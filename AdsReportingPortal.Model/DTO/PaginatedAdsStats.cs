using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsReportingPortal.Model.DTO
{
    public class PaginatedAdsStats
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<MetaAdsAllDailyResponse> MetaStats { get; set; }
    }
}

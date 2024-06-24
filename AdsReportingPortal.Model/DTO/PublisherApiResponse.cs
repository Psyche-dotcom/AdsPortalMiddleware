using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AdsReportingPortal.Model.DTO
{
    public class PublisherApiResponse
    {
        public List<PublisherData> data { get; set; }
    }
  

    public class PublisherData
    {
        public string account_id { get; set; }
        public string campaign_id { get; set; }
        public string date_start { get; set; }
        public string date_stop { get; set; }
        public string impressions { get; set; }
        public string spend { get; set; }
        public string publisher_platform { get; set; }
    }
}

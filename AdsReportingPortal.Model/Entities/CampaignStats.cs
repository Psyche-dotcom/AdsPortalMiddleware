using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsReportingPortal.Model.Entities
{
    public class CampaignStats : BaseEntity
    {
        public string CampaignsId { get; set; }
        public string Impression {  get; set; }
        public string Budget { get; set; }
        
    }
}

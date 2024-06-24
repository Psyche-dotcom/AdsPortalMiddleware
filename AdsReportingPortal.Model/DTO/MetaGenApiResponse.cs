using AdsReportingPortal.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsReportingPortal.Model.DTO
{

    public class MetaGenApiResponse
    {
        public string cpm { get; set; }
        public string impressions { get; set; }
        public string reach { get; set; }
        public string clicks { get; set; }
        public string ctr { get; set; }
        public string spend { get; set; }
        public List<MetaAction> actions { get; set; }
        public List<CostPerActionType> cost_per_action_type { get; set; }
        public string frequency { get; set; }
        public List<VideoPlayAction> video_play_actions { get; set; }
        public string unique_clicks { get; set; }
        public string date_start { get; set; }
        public string date_stop { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string platform { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsReportingPortal.Model.DTO
{
    public class GetCampaignsResponse
    {
        [JsonProperty("data")]
        public List<AdsCampaignData> Data { get; set; }

        [JsonProperty("paging")]
        public CampaingnPagingInfo Paging { get; set; }
    }

    public class AdsCampaignData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("objective")]
        public string Objective { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("stop_time")]
        public DateTime? StopTime { get; set; }

        [JsonProperty("budget_remaining")]
        public string BudgetRemaining { get; set; }

        [JsonProperty("daily_budget")]
        public string DailyBudget { get; set; }
    }

    public class CampaingnPagingInfo
    {
        [JsonProperty("cursors")]
        public CursorsInfo Cursors { get; set; }
    }

    public class CursorsInfo
    {
        [JsonProperty("before")]
        public string Before { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }
    }
}

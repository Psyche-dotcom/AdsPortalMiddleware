using System.Text.Json.Serialization;

namespace AdsReportingPortal.Model.DTO
{
    public class ApiResponse
    {
        [JsonPropertyName("data")]
        public List<CampaignData> Data { get; set; }

        [JsonPropertyName("paging")]
        public PagingInfo Paging { get; set; }
    }

    public class CampaignData
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("campaign_id")]
        public string CampaignId { get; set; }

        [JsonPropertyName("date_start")]
        public string DateStart { get; set; }

        [JsonPropertyName("date_stop")]
        public string DateStop { get; set; }

        [JsonPropertyName("impressions")]
        public string Impressions { get; set; }

        [JsonPropertyName("spend")]
        public string Spend { get; set; }
    }

    public class PagingInfo
    {
        [JsonPropertyName("cursors")]
        public Cursors Cursors { get; set; }
    }

    public class Cursors
    {
        [JsonPropertyName("before")]
        public string Before { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }
    }

}

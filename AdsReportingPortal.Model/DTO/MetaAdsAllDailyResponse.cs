namespace AdsReportingPortal.Model.DTO
{
    public class MetaAdsAllDailyResponse
    {
        public string cpm { get; set; }
        public string impressions { get; set; }
        public string reach { get; set; }
        public string clicks { get; set; }
        public string ctr { get; set; }
        public string spend { get; set; }
        public string frequency { get; set; }
        public DateTime stat_date { get; set; }
        public string channel { get; set; } = "Meta";
        public string unique_clicks { get; set; }
        public string date_start { get; set; }
        public string date_stop { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
    }
}

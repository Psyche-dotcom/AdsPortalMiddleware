namespace AdsReportingPortal.Model.Entities
{
    public class Campaigns : BaseEntity
    {
        public string CampaignId { get; set; }
        public bool isSuspended { get; set; } = false;
        public string CampaignName { get; set;}
    }
}

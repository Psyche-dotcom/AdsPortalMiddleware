namespace AdsReportingPortal.Model.Entities
{
    public class ConfirmEmailToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Token { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

namespace AdsReportingPortal.Model.DTO
{
    public class Business
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AdAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int AccountStatus { get; set; }
        public string Currency { get; set; }
        public string AmountSpent { get; set; }
        public Business Business { get; set; }
        public DateTime CreatedTime { get; set; }
        public string TimezoneName { get; set; }
        public string Owner { get; set; }
    }

    public class PagingCursors
    {
        public string Before { get; set; }
        public string After { get; set; }
    }

    public class Paging
    {
        public PagingCursors Cursors { get; set; }
    }

    public class GetAdsAccountResponse
    {
        public List<AdAccount> Data { get; set; }
        public Paging Paging { get; set; }
    }

}

using Microsoft.AspNetCore.Identity;

namespace AdsReportingPortal.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ConfirmEmailToken ConfirmEmailToken { get; set; }
    }
}

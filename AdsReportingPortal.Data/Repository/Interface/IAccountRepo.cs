using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;

namespace AdsReportingPortal.Data.Repository.Interface
{
    public interface IAccountRepo
    {

        Task<ApplicationUser> SignUpAsync(ApplicationUser user, string Password);

        Task<bool> CheckAccountPassword(ApplicationUser user, string password);

        int GenerateConfirmEmailToken();
        Task<bool> DeleteUserToken(ConfirmEmailToken token);
        Task<ConfirmEmailToken> retrieveUserToken(string userid);
        Task<ConfirmEmailToken> SaveGenerateConfirmEmailToken(ConfirmEmailToken emailToken);

        Task<bool> CheckEmailConfirmed(ApplicationUser user);

        Task<bool> AddRoleAsync(ApplicationUser user, string Role);

        Task<string> ForgotPassword(ApplicationUser user);

        Task<bool> ConfirmEmail(string token, ApplicationUser user);

        Task<bool> RemoveRoleAsync(ApplicationUser user, IList<string> role);

        Task<ResetPassword> ResetPasswordAsync(ApplicationUser user, ResetPassword resetPassword);

        Task<bool> RoleExist(string Role);

        Task<ApplicationUser?> FindUserByEmailAsync(string email);

        Task<ApplicationUser> FindUserByIdAsync(string id);

        Task<bool> UpdateUserInfo(ApplicationUser applicationUser);

        Task<IList<string>> GetUserRoles(ApplicationUser user);

        Task<bool> DeleteUserByEmail(ApplicationUser user);
    }
}

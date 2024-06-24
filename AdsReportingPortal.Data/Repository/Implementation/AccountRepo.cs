using AdsReportingPortal.Data.Context;
using AdsReportingPortal.Data.Repository.Interface;
using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AdsReportingPortal.Data.Repository.Implementation
{
    public class AccountRepo : IAccountRepo
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProjectDbContext _context;

        public AccountRepo(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ProjectDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<bool> AddRoleAsync(ApplicationUser user, string Role)
        {
            var AddRole = await _userManager.AddToRoleAsync(user, Role);
            if (AddRole.Succeeded)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveRoleAsync(ApplicationUser user, IList<string> role)
        {
            var removeRole = await _userManager.RemoveFromRolesAsync(user, role);
            if (removeRole.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            var getRoles = await _userManager.GetRolesAsync(user);
            if (getRoles != null)
            {
                return getRoles;
            }
            return null;
        }

        public async Task<bool> RoleExist(string Role)
        {
            var check = await _roleManager.RoleExistsAsync(Role);
            return check;
        }
        public async Task<bool> ConfirmEmail(string token, ApplicationUser user)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUserByEmail(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<ApplicationUser?> FindUserByEmailAsync(string email)
        {
            var findUser = await _userManager.FindByEmailAsync(email);
            if (findUser == null)
            {
                return null;
            }
            return findUser;
        }

        public async Task<ApplicationUser> FindUserByIdAsync(string id)
        {
            var findUser = await _userManager.FindByIdAsync(id);
            return findUser;
        }

        public async Task<string> ForgotPassword(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }
        public async Task<bool> CheckEmailConfirmed(ApplicationUser user)
        {
            var checkConfirm = user.EmailConfirmed == true;
            return checkConfirm;
        }

        public async Task<bool> CheckAccountPassword(ApplicationUser user, string password)
        {
            var checkUserPassword = await _userManager.CheckPasswordAsync(user, password);
            return checkUserPassword;
        }

        public async Task<ResetPassword> ResetPasswordAsync(ApplicationUser user, ResetPassword resetPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded)
            {
                return resetPassword;
            }
            return null;
        }

        public async Task<ApplicationUser> SignUpAsync(ApplicationUser user, string Password)
        {
            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                return user;
            }
            return null;
        }

        public async Task<bool> UpdateUserInfo(ApplicationUser applicationUser)
        {
            var updateUserInfo = await _userManager.UpdateAsync(applicationUser);
            if (updateUserInfo.Succeeded)
            {
                return true;
            }
            return false;
        }

        public int GenerateConfirmEmailToken()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 1000000);
            return randomNumber;
        }
        public async Task<ConfirmEmailToken> SaveGenerateConfirmEmailToken(ConfirmEmailToken emailToken)
        {
            var saveToken = await _context.ConfirmEmailTokens.AddAsync(emailToken);
            var save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                return emailToken;
            }
            return null;
        }
        public async Task<ConfirmEmailToken> retrieveUserToken(string userid)
        {
            return await _context.ConfirmEmailTokens.FirstOrDefaultAsync(u => u.UserId == userid);
        }
        public async Task<bool> DeleteUserToken(ConfirmEmailToken token)
        {
            _context.ConfirmEmailTokens.Remove(token);
            var save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                return true;
            }
            return false;
        }
    }
}

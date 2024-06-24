using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdsReportingPortal.Controllers
{
    [Route("api/user/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(SignUp signUp)
        {
            var registerUser = await _accountService.RegisterUser(signUp, "USER");
            if (registerUser.StatusCode == 200)
            {
                return Ok(registerUser);
            }
            else if (registerUser.StatusCode == 404)
            {
                return NotFound(registerUser);
            }
            else
            {
                return BadRequest(registerUser);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(SignInModel signIn)
        {
            var loginUser = await _accountService.LoginUser(signIn);
            if (loginUser.StatusCode == 200)
            {
                return Ok(loginUser);
            }
            else if (loginUser.StatusCode == 404)
            {
                return NotFound(loginUser);
            }
            else
            {
                return BadRequest(loginUser);
            }
        }

        [HttpPost("forgot_password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _accountService.ForgotPassword(email);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var result = await _accountService.ResetUserPassword(resetPassword);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        /*[HttpPost("confirm-email/{email}")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailTokenDto token, string email)
        {

            var result = await _accountService.ConfirmEmailAsync(token.token, email);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }*/
    }

}

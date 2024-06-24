using AdsReportingPortal.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdsReportingPortal.Api.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        private readonly IAccessTokenService _accessTokenService;

        public AccessTokenController(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddToken([FromBody]string token)
        {
            var result = await _accessTokenService.AddToken(token);
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
     
    }
}

using AdsReportingPortal.Api.Service.Interface;
using AdsReportingPortal.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdsReportingPortal.Api.Controllers
{
    [Route("api/campaigns")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignsService _campaignsService;

        public CampaignController(ICampaignsService campaignsService)
        {
            _campaignsService = campaignsService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddCamapaign(AddCampaignDto req)
        {
            var result = await _campaignsService.AddCampaign(req.CampaignId, req.CampaignName);
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

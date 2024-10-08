﻿using AdsReportingPortal.Api.Service.Implementation;
using AdsReportingPortal.Api.Service.Interface;
using AdsReportingPortal.Model.DTO;
using AdsReportingPortal.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AdsReportingPortal.Api.Controllers
{
    [Route("api/ads/stats")]
    [ApiController]
    public class AdsStatsController : ControllerBase
    {
        private readonly IAdsStatService _adsStatService;

        public AdsStatsController(IAdsStatService adsStatService)
        {
            _adsStatService = adsStatService;
        }



        [HttpGet("ads_account/campaigns")]
        public async Task<IActionResult> GetAllCampaingn(string adsid)
        {
            var result = await _adsStatService.GetCampaignsAsync(adsid);
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
        [HttpGet("ads_account/all")]
        public async Task<IActionResult> GetAllAdsAccount()
        {
            var result = await _adsStatService.GetAdAccountsAsync();
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

        [HttpPost("meta/impression/daily/date")]
        public async Task<IActionResult> GetDailyStats (DailyStats req)
        {
            var result = await _adsStatService.GetImpressionAggregate(req.Date);
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
        [HttpPost("meta/stats/daily/all")]
        public async Task<IActionResult> GetDailyStats (DailyStats req, int pageSize, int pagenumber)
        {
            var result = await _adsStatService.GetStatsPaginatedAggregate(req.Date,pagenumber,pageSize);
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




        [HttpPost("meta/stats/daily/date")]
        public async Task<IActionResult> GetDailyOtherStats([FromBody]DailyStats req, [FromQuery] int type)
        {
            var result = await _adsStatService.GetOthersAggregate(req.Date,type);
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.External;
using backend.Dtos.Rate;
using backend.Dtos.WatchlistItem;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ratesController : ControllerBase
	{
		// Db context
		private readonly RateRefreshService _rateRefreshService;
		private readonly LatestRateService _latestRateService;
		private readonly HistoryRateService _historyRateService;
		private readonly ILogger<ratesController> _logger;



		public ratesController(RateRefreshService rateRefreshService, LatestRateService latestRateService, HistoryRateService historyRateService, ILogger<ratesController> logger)
		{
			_rateRefreshService = rateRefreshService;
			_latestRateService = latestRateService;
			_historyRateService = historyRateService;
			_logger = logger;
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshRate()
		{

			var result = await _rateRefreshService.RefreshRateAsync();
			if (!result.Success)
			{
				return StatusCode(500, new
				{
					message = result.Message,
					saved = 0
				});
			}

			return Accepted(new
			{
				message = result.Message,
				created = result.TotalCreatedRecords,
				updated = result.TotalUpdatedRecords
			});
		}

		[HttpGet("latest")]
		public async Task<IActionResult> GetLastestRate([FromQuery] string baseCur, [FromQuery] string quoteCur)
		{
			if (string.IsNullOrWhiteSpace(baseCur) || string.IsNullOrWhiteSpace(quoteCur))
			{
				return BadRequest(new
				{
					message = "Both base and quote currency are required."
				});
			}

			var result = await _latestRateService.GetLatestRateAsync(baseCur, quoteCur);
			if (result == null)
			{
				return NotFound(new
				{
					message = $"Could not fetch live rate for {baseCur}/{quoteCur}. Check the currency codes and try again."
				});
			}

			return Ok(new
			{
				status = "success",
				message = "Lastest exchange rates fetched successfully",
				count = 1,
				records = result
			});
		}

		[HttpGet("history")]
		public async Task<IActionResult> GetHistoryRateAsync([FromQuery] string baseCur, [FromQuery] string quoteCur, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
		{
			if (string.IsNullOrWhiteSpace(baseCur) || string.IsNullOrWhiteSpace(quoteCur))
			{
				return BadRequest(new
				{
					message = "Both base and quotes are required."
				});
			}

			if (startDate >= endDate)
			{
				return BadRequest(new { message = "startDate must be before endDate." });
			}

			var results = await _historyRateService.GetHistoryRateAsync(baseCur, quoteCur, startDate, endDate);
			return Ok(new
			{
				status = "success",
				message = "Historical exchange rates fetched successfully",
				count = results.Count,
				records = results
			});
		}
	}
}
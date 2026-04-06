using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.External;
using backend.Dtos.Rate;
using backend.Dtos.WatchlistItem;
using backend.Helpers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RatesController : ControllerBase
	{
		// Db context
		private readonly RateRefreshService _rateRefreshService;
		private readonly LatestRateService _latestRateService;
		private readonly HistoryRateService _historyRateService;
		private readonly ILogger<RatesController> _logger;



		public RatesController(RateRefreshService rateRefreshService, LatestRateService latestRateService, HistoryRateService historyRateService, ILogger<RatesController> logger)
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
				return StatusCode(500, ApiResponse<object>.ServerError(result.Message));
			}

			return Accepted(ApiResponse<object?>.Success(
				data: null,
				message: result.Message,
				statusCode: 200
			));
		}

		[HttpGet("latest")]
		public async Task<IActionResult> GetLastestRate([FromQuery] string baseCur, [FromQuery] string quoteCur)
		{
			if (string.IsNullOrWhiteSpace(baseCur) || string.IsNullOrWhiteSpace(quoteCur))
			{
				return BadRequest(ApiResponse<object?>.Error(null, "Both base and quote currency are required.", 400));
			}

			var result = await _latestRateService.GetLatestRateAsync(baseCur, quoteCur);
			if (result == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("Both base and quote currency are required."));
			}

			return Ok(ApiResponse<object?>.Success(
				data: result,
				message: "Latest exchange rates fetched successfully",
				statusCode: 200
			));
		}

		[HttpGet("history")]
		public async Task<IActionResult> GetHistoryRateAsync([FromQuery] string baseCur, [FromQuery] string quoteCur, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
		{
			if (string.IsNullOrWhiteSpace(baseCur) || string.IsNullOrWhiteSpace(quoteCur))
			{
				return BadRequest(ApiResponse<object?>.Error(null, "Both base and quote currency are required.", 400));
			}

			if (startDate >= endDate)
			{
				return BadRequest(ApiResponse<object?>.Error(null, "startDate must be before endDate.", 400));
			}

			var results = await _historyRateService.GetHistoryRateAsync(baseCur, quoteCur, startDate, endDate);
			return Ok(ApiResponse<object?>.Success(
				data: results,
				message: "Historical exchange rates fetched successfully",
				statusCode: 200
			));
		}
	}
}
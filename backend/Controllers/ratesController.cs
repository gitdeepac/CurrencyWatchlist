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
		private readonly ILogger<ratesController> _logger;



		public ratesController(RateRefreshService rateRefreshService, ILogger<ratesController> logger)
		{
			_rateRefreshService = rateRefreshService;
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
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Rate;
using backend.Helpers;
using backend.Mappers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{

	// Manages alert rules.
	[Route("api/[controller]")]
	[ApiController]
	public class AlertRuleController : ControllerBase
	{

		// Injected via DI
		private readonly ApplicationDbContext _context;
		private readonly RateRefreshService _rateRefreshService;
		private readonly ILogger<AlertRuleController> _logger;
		public AlertRuleController(ApplicationDbContext context, RateRefreshService rateRefreshService, ILogger<AlertRuleController> logger)
		{
			_context = context;
			_rateRefreshService = rateRefreshService;
			_logger = logger;
		}

		// Returns all records
		[HttpGet]
		public async Task<IActionResult> GetAllRule()
		{
			var alertRuleListDto = await _context.AlertRule.ToListAsync();

			return Ok(ApiResponse<object?>.Success(
				data: alertRuleListDto,
				message: "Successfully fetch alert rule list",
				statusCode: 200
			));
		}

		// Returns single record, 404 if missing
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			var alertRuleList = await _context.AlertRule.FirstOrDefaultAsync(x => x.Id == id); ;

			if (alertRuleList == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}

			return Ok(ApiResponse<object?>.Success(alertRuleList, "Successfully fetch alert rule", 200));
		}

		// Post new alert rule from request body
		[HttpPost]
		public async Task<IActionResult> CreateAlertRule([FromBody] CreateAlertRuleRequestDto createRateAlertRequestDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var rateAlertRuleModel = createRateAlertRequestDto.ToRateAlertRuleFromCreateDTO();
			await _context.AlertRule.AddAsync(rateAlertRuleModel);
			await _context.SaveChangesAsync();


			return CreatedAtAction(nameof(GetById), new { id = rateAlertRuleModel.Id },
				ApiResponse<object?>.Success(rateAlertRuleModel.ToAlertRuleListDto(), "Successfully created alert rule.", 201));
		}


		// Hard delete — no soft delete
		[HttpDelete]
		[Route("{Id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int Id)
		{
			var alertListModel = await _context.AlertRule.FirstOrDefaultAsync(x => x.Id == Id);

			if (alertListModel == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}

			_context.AlertRule.Remove(alertListModel);
			await _context.SaveChangesAsync();

			return Ok(ApiResponse<object?>.Success(null, $"Successfully Deleted Alert {Id}", 200));
		}

		[HttpPost("{id:int}/evaluate")]
		public async Task<IActionResult> EvaluateAlert([FromRoute] int id)
		{

			// STEPS
			// 1. Check the alert is avaliable into database with watchlist id
			// 2. Update the rate through service into database
			// 3. Get the watchlist from Alert
			// 4. Check the condition matched
			// 5. If matched then create alert event.

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var alertRule = await _context.AlertRule.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (alertRule == null)
			{
				return NotFound(ApiResponse<object?>.ServerError(
					$"Failed to fetch the alertrule for : {id}"));
			}

			var refreshResult = await _rateRefreshService.RefreshRateAsync(); // 
			if (!refreshResult.Success)
			{
				return NotFound(ApiResponse<object?>.ServerError(
					$"Failed to refresh rate: {refreshResult.Message}"));
			}

			var watchlistItem = await _context.WatchlistItems
									.FirstOrDefaultAsync(x => x.Id == alertRule.WatchlistItemId);

			if (watchlistItem == null)
				return NotFound(ApiResponse<object?>.ServerError(
					$"Watchlist item not found."));

			var rateSnapShot = await _context.RateSnapShot
					.FirstOrDefaultAsync(r => r.QuoteCurrency == watchlistItem.QuoteCurrency);
			if (rateSnapShot == null)
			{
				return NotFound(ApiResponse<object?>.ServerError(
					$"Rate snapshot not found."));
			}
			// now check condition
			decimal rateValue = rateSnapShot.Rate;

			var results = new List<object>();

			decimal thresholdValue = decimal.Parse(alertRule.Threshold);

			var condition = alertRule.Condition.Trim();

			bool isTriggered = false;

			if (condition == ">")
				isTriggered = rateValue > thresholdValue;
			else if (condition == "<")
				isTriggered = rateValue < thresholdValue;
			else if (condition == ">=")
				isTriggered = rateValue >= thresholdValue;
			else if (condition == "<=")
				isTriggered = rateValue <= thresholdValue;
			else if (condition == "==")
				isTriggered = rateValue == thresholdValue;
			else if (condition == "!=")
				isTriggered = rateValue != thresholdValue;

			if (isTriggered)
			{
				var alertEvent = new AlertEvent
				{
					AlertRuleId = alertRule.Id,
					Rate = rateValue.ToString(),
					TriggerAt = DateTime.UtcNow
				};

				_context.AlertEvent.Add(alertEvent);
			}

			results.Add(new
			{
				AlertRuleId = alertRule.Id,
				CurrentRate = rateValue,
				Threshold = thresholdValue,
				Condition = alertRule.Condition,
				Triggered = isTriggered
			});

			await _context.SaveChangesAsync();

			return Ok(
				ApiResponse<object?>.Success(
					results,
					"Alert evaluation completed"
				)
			);
		}
	}
}
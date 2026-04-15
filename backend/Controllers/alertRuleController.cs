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
		public readonly ApplicationDbContext _context;
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
			if (!ModelState.IsValid)
				return BadRequest(ModelState);


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
			var alertRuleList = await _context.AlertRule.FindAsync(id);

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
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

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
			// 1. Check the alert is avaliable into database
			// 2. Update the rate through service into database
			// 3. Get the watchlist from Alert
			// 4. Check the condition matched
			// 5. If matched then create alert event.

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var alertRule = await _context.AlertRule.FindAsync(id);
			if (alertRule == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("Alert rule not found"));
			}
			_logger.LogWarning("Wathlist Id: {watchlistId}", alertRule.WatchlistItemId);
			var refreshResult = await _rateRefreshService.RefreshRateAsync(); // 
			if (!refreshResult.Success)
			{
				return StatusCode(500, ApiResponse<object?>.ServerError(
					$"Failed to refresh rate: {refreshResult.Message}"));
			}
			var rateSnapShot = await _context.WatchlistItems
				.Where(x => x.Id == alertRule.WatchlistItemId)
				.Join(
					_context.RateSnapShot,
					watchlistItem => watchlistItem.QuoteCurrency,
					rateSnapshot => rateSnapshot.QuoteCurrency,
					(watchlistItem, rateSnapshot) => rateSnapshot
				)
				.FirstOrDefaultAsync();
			if (rateSnapShot == null)
			{
				return NotFound("Rate snapshot not found.");
			}
			// now check condition
			decimal thresholdValue = decimal.Parse(alertRule.Threshold);
			decimal rateValue = rateSnapShot.Rate;
			var result = alertRule.Condition.Trim() switch
			{
				">" => rateValue > thresholdValue,
				"<" => rateValue < thresholdValue,
				">=" => rateValue >= thresholdValue,
				"<=" => rateValue <= thresholdValue,
				"==" => rateValue == thresholdValue,
				"!=" => rateValue != thresholdValue,
				_ => false
			};

			if (result)
			{
				var alertEvent = new AlertEvent
				{
					AlertRuleId = alertRule.Id,
					Rate = rateValue.ToString(),
					TriggerAt = DateTime.UtcNow
				};

				_context.AlertEvent.Add(alertEvent);
				await _context.SaveChangesAsync();
			}

			return Ok(
				ApiResponse<object?>.Success(
					new
					{
						AlertRuleId = alertRule.Id,
						CurrentRate = rateValue,
						Threshold = thresholdValue,
						Condition = alertRule.Condition,
						Triggered = result
					},
					result ? "Alert triggered successfully." : "Alert evaluated successfully.",
					200
				)
			);
		}
	}
}
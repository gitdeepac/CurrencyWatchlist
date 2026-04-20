using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Alert;
using backend.Helpers;
using backend.Interfaces;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;

public class AlertEvaluationService : IAlertEvaluationService
{
	private readonly ApplicationDbContext _context;
	private readonly RateRefreshService _rateRefreshService;
	private readonly ILogger<RateRefreshService> _logger;

	public AlertEvaluationService(ApplicationDbContext context, RateRefreshService rateRefreshService, ILogger<RateRefreshService> logger)
	{
		_context = context;
		_rateRefreshService = rateRefreshService;
		_logger = logger;
	}

	public async Task<AlertEvaluationResult?> EvaluateAsync(int id)
	{

		// STEPS
		// 1. Check the alert is avaliable into database with watchlist id
		// 2. Update the rate through service into database
		// 3. Get the watchlist from Alert
		// 4. Check the condition matched
		// 5. If matched then create alert event.
		var alertRule = await _context.AlertRule
			.FirstOrDefaultAsync(x => x.Id == id);

		if (alertRule == null)
			return null;

		var refreshResult = await _rateRefreshService.RefreshRateAsync();
		if (!refreshResult.Success)
			return null;

		var watchlistItem = await _context.WatchlistItems
			.FirstOrDefaultAsync(x => x.Id == alertRule.WatchlistItemId);

		if (watchlistItem == null)
			return null;

		var rateSnapShot = await _context.RateSnapShot
			.FirstOrDefaultAsync(r => r.QuoteCurrency == watchlistItem.QuoteCurrency);

		if (rateSnapShot == null)
			return null;

		decimal rateValue = rateSnapShot.Rate;
		decimal thresholdValue = decimal.Parse(alertRule.Threshold);
		string condition = alertRule.Condition.Trim();

		bool isTriggered = EvaluateCondition(rateValue, thresholdValue, condition);

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

		await _context.SaveChangesAsync();

		var result =  new AlertEvaluationResult
		{
			AlertRuleId = alertRule.Id,
			CurrentRate = rateValue,
			Threshold = thresholdValue,
			Condition = condition,
			Triggered = isTriggered
		};

		return result;
	}

	private bool EvaluateCondition(decimal rate, decimal threshold, string condition)
	{
		return condition switch
		{
			">" => rate > threshold,
			"<" => rate < threshold,
			">=" => rate >= threshold,
			"<=" => rate <= threshold,
			"==" => rate == threshold,
			"!=" => rate != threshold,
			_ => false
		};
	}
}

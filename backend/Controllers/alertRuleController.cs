using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Rate;
using backend.Helpers;
using backend.Interfaces;
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
		private readonly IAlertEvaluationService _alertService;
		private readonly IAlertRuleRepository _alertRepository;
		private readonly ILogger<AlertRuleController> _logger;
		public AlertRuleController(IAlertEvaluationService alertService, IAlertRuleRepository alertRepository, ILogger<AlertRuleController> logger)
		{
			_alertService = alertService;
			_alertRepository = alertRepository;
			_logger = logger;
		}

		// Returns all records
		[HttpGet]
		public async Task<IActionResult> GetAllRule()
		{
			_logger.LogInformation("Trigger Repo to get all rules.");
			var alertResults = await _alertRepository.GetAllRuleAsync();
			
			return Ok(ApiResponse<object?>.Success(
				data: alertResults,
				message: "Successfully fetch alert rule list",
				statusCode: 200
			));
		}

		// Returns single record, 404 if missing
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			_logger.LogInformation("Trigger Repo to get data by Id.");
			var alertRuleResult = await _alertRepository.GetByIdAsync(id);
			
			if (alertRuleResult == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}

			return Ok(ApiResponse<object?>.Success(alertRuleResult, "Successfully fetch alert rule", 200));
		}

		// Post new alert rule from request body
		[HttpPost]
		public async Task<IActionResult> CreateAlertRule([FromBody] CreateAlertRuleRequestDto createRateAlertRequestDto)
		{
			_logger.LogInformation("Trigger to get the alertRules from Mappers");
			var rateAlertRuleModel = createRateAlertRequestDto.ToRateAlertRuleFromCreateDTO();

			_logger.LogInformation("Create Method triggered.");
			await _alertRepository.CreateAlertRuleAsync(rateAlertRuleModel);

			return CreatedAtAction(nameof(GetById), new { id = rateAlertRuleModel.Id },
				ApiResponse<object?>.Success(rateAlertRuleModel.ToAlertRuleListDto(), "Successfully created alert rule.", 201));
		}


		// Hard delete — no soft delete
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			_logger.LogInformation("Delete Method Triggerd.");
			var alertListModel = await _alertRepository.DeleteAsync(id);

			if (alertListModel == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}
			return Ok(ApiResponse<object?>.Success(null, $"Successfully Deleted Alert {id}", 200));
		}

		[HttpPost("{id:int}/evaluate")]
		public async Task<IActionResult> EvaluateAlert([FromRoute] int id)
		{
			_logger.LogInformation("Evaluate Method Trigger.");
			var result = await _alertService.EvaluateAsync(id);

			return Ok(
				ApiResponse<object?>.Success(
					result,
					"Alert evaluation completed"
				)
			);
		}
	}
}
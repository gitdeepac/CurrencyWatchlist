using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Rate;
using backend.Helpers;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AlertRuleController : ControllerBase
	{

		public readonly ApplicationDbContext _context;
		public AlertRuleController(ApplicationDbContext context)
		{
			_context = context;
		}
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
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Alert;
using backend.Dtos.Rate;
using backend.Models;

namespace backend.Mappers
{
	public static class RateAlertRuleMapper
	{
		public static AlertRuleListDto ToAlertRuleListDto(this AlertRule alertRuleList)
		{
			return new AlertRuleListDto
			{
				WatchlistItemId = alertRuleList.WatchlistItemId,
				Threshold = alertRuleList.Threshold,
				Condition = alertRuleList.Condition,
				IsActvie = alertRuleList.IsActvie,
				CreateAt = alertRuleList.CreateAt,
			};
		}
		public static AlertRule ToRateAlertRuleFromCreateDTO(this CreateAlertRuleRequestDto rateAlertRequestDto)
		{
			return new AlertRule
			{
				WatchlistItemId = rateAlertRequestDto.WatchlistItemId,
				Condition = rateAlertRequestDto.Condition,
				Threshold = rateAlertRequestDto.Threshold,
				IsActvie = rateAlertRequestDto.IsActive,
				CreateAt = DateTime.Now
			};
		}
	}
}
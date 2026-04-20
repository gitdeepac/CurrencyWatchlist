using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Alert;
using backend.Helpers;
using backend.Models;


namespace backend.Interfaces
{
	public interface IAlertEvaluationService
	{
		Task<AlertEvaluationResult?> EvaluateAsync(int alertId);
	}
}
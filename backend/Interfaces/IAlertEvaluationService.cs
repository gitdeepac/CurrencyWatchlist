using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;

namespace backend.Interfaces
{
	public interface IAlertEvaluationService
	{
		Task<ApiResponse<object?>> EvaluateAsync(int alertId);
	}
}
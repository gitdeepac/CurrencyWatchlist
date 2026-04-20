using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Alert
{
	public class AlertEvaluationResult
	{
		public int AlertRuleId { get; set; }
		public decimal CurrentRate { get; set; }
		public decimal Threshold { get; set; }
		public string Condition { get; set; }
		public bool Triggered { get; set; }
	}
}
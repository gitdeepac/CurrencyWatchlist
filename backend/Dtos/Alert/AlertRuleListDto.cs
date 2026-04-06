using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Alert
{
	public class AlertRuleListDto
	{
		public int? WatchlistItemId { get; set; }
		public string Condition { get; set; } = String.Empty;
		public string Threshold { get; set; } = String.Empty;
		public Boolean IsActvie { get; set; } = true;
		public DateTime CreateAt { get; set; } = DateTime.UtcNow;
	}
}
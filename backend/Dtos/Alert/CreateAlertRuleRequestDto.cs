using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Alert
{
	public class CreateAlertRuleRequestDto
	{
		[Required]
		public int? WatchlistItemId { get; set; }
		[Required]
		public string Condition { get; set; } = String.Empty;
		[Required]
		public string Threshold { get; set; } = String.Empty;
		[Required]
		public Boolean IsActive { get; set; } = true;
		public DateTime CreateAt { get; set; } = DateTime.UtcNow;
	}
}
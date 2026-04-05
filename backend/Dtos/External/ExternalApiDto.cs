using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.External
{
	public class ExternalApiDto
	{
		public int amount { get; set; }
		public String BaseCurrency { get; set; } = string.Empty;
		public DateOnly fetchedDate { get; set; }
		public String QuoteCurrency { get; set; } = string.Empty;
		public Decimal rates { get; set; }

	}
}
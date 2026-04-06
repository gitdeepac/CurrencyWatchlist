using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.External
{
	public class ExternalApiRawResponse
	{
		public DateOnly Date { get; set; }  // "2026-04-05"
		public string Base { get; set; } = string.Empty;  // "AUD"
		public string Quote { get; set; } = string.Empty;  // "INR"
		public decimal Rate { get; set; }                  // 64.207
	}
}
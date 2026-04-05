using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.External
{
	public class ExternalApiRawResponse
	{
		public int Amount { get; set; }
		public string Base { get; set; } = string.Empty;
		public string Date { get; set; } = string.Empty;
		public Dictionary<string, decimal> Rates { get; set; } = new();
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.WatchlistItem
{
	public class WatchlistItemSummaryDto
	{
		public String BaseCurrency { get; set; } = string.Empty;
		public String QuoteCurrency { get; set; } = string.Empty;
	}
}
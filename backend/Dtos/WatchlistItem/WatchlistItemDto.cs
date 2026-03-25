using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.WatchlistItem
{
    public class WatchlistItemDto
    {
        public int Id { get; set; }
        public int WatchlistId { get; set; }
        public string BaseCurrency { get; set; } = string.Empty;
        public string QuoteCurrency { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
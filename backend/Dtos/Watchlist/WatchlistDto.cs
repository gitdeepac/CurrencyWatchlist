using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.WatchlistItem;

namespace backend.Dtos.Watchlist
{
    public class WatchlistDto
    {
      public int Id { get; set; }
      public string Name { get; set; } = string.Empty;
      public DateTime CreateAt { get; set; } = DateTime.Now;

      public List<WatchlistItemDto> Items { get; set; } = new List<WatchlistItemDto>();
    }
}
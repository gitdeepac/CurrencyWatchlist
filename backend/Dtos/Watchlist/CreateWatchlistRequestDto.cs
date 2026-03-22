using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Watchlist
{
    public class CreateWatchlistRequestDto
    {
      public string Name { get; set; } = string.Empty;
      public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
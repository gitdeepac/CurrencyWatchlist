using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Watchlist;
using backend.Models;

namespace backend.Mappers
{
  public static class WatchlistMappers
  {
    public static WatchlistDto ToWatchlistDto(this Watchlist watchlistModel)
    {
      return new WatchlistDto
      {
        Id = watchlistModel.Id,
        Name = watchlistModel.Name
      };
    }

    public static Watchlist ToWatchlistFromCreateDTO(this CreateWatchlistRequestDto watchlistRequestDto)
    {
      return new Watchlist
      {
        Name = watchlistRequestDto.Name,
        CreateAt = DateTime.UtcNow
      };
    }
  }
}
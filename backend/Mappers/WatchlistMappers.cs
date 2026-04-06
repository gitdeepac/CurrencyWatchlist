using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Watchlist;
using backend.Dtos.WatchlistItem;
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
				Name = watchlistModel.Name,
				CreateAt = watchlistModel.CreateAt,
				Items = watchlistModel.Items.Select(item => new WatchlistItemDto
				{
					Id = item.Id,
					BaseCurrency = item.BaseCurrency,
					QuoteCurrency = item.QuoteCurrency,
					WatchlistId = item.WatchlistId ?? 0,
				}).ToList()
			};
		}

		public static Watchlist ToWatchlistFromCreateDTO(this CreateWatchlistRequestDto watchlistRequestDto)
		{
			return new Watchlist
			{
				Name = watchlistRequestDto.Name.Trim(),
				CreateAt = DateTime.UtcNow
			};
		}
	}
}
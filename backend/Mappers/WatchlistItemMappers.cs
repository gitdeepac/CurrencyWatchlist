using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.WatchlistItem;
using backend.Models;

namespace backend.Mappers
{
	public static class WatchlistItemMappers
	{
		public static WatchlistItemDto ToWatchlistItemDto(this WatchlistItems watchlistItemModel)
		{
			return new WatchlistItemDto
			{
				Id = watchlistItemModel.Id,
				WatchlistId = watchlistItemModel.WatchlistId ?? 0,
				BaseCurrency = watchlistItemModel.BaseCurrency,
				QuoteCurrency = watchlistItemModel.QuoteCurrency,
			};
		}

		public static WatchlistItems ToWatchlistItemFromCreateDTO(this CreateWatchlistItemRequestDto watchlistItemRequestDto, int watchlistId)
		{
			return new WatchlistItems
			{
				WatchlistId = watchlistId,
				BaseCurrency = watchlistItemRequestDto.BaseCurrency.ToUpper().Trim(),
				QuoteCurrency = watchlistItemRequestDto.QuoteCurrency.ToUpper().Trim(),
				CreateAt = DateTime.Now
			};
		}
	}
}
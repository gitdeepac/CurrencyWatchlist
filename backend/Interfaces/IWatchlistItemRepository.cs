using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.WatchlistItem;
using backend.Models;

namespace backend.Interfaces
{
	public interface IWatchlistItemRepository
	{
		Task<WatchlistItemDto?> GetByIdAsync(int id, int watchlistId); 

		Task<List<WatchlistItemDto>> GetAllAsync(int watchlistId);
		Task<WatchlistItems> CreateWatchlistAsync(WatchlistItems watchlistModel);

		Task<WatchlistItems?> DeleteWatchlistAsync(int id);

		Task<bool> ExistsAsync(int watchlistId, string baseCurrency, string quoteCurrency);
	}
}
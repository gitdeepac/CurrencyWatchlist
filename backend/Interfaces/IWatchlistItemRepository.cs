using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
	public interface IWatchlistItemRepository
	{
		Task<WatchlistItems?> GetByIdAsync(int id, int watchlistId); 

		Task<List<WatchlistItems>> GetAllAsync(int watchlistId);
		Task<WatchlistItems> CreateWatchlistAsync(WatchlistItems watchlistModel);

		Task<WatchlistItems?> DeleteWatchlistAsync(int id);

		Task<bool> ExistsAsync(int watchlistId, string baseCurrency, string quoteCurrency);
	}
}
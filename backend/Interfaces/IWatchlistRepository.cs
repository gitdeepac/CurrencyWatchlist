using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface IWatchlistRepository
    {
        Task<List<Watchlist>>  GetAllAsync();
		Task<Watchlist?> GetByIdAsync(int id); // First or default and can be null so used ?
		Task<Watchlist> CreateWatchlistAsync(Watchlist watchlistModel);

		Task<Watchlist?> DeleteWatchlistAsync(int id);
		
    }
}
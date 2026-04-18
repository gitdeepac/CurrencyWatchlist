using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
	public class WatchlistRepository : IWatchlistRepository
	{
		private readonly ApplicationDbContext _context;
		public WatchlistRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Watchlist> CreateWatchlistAsync(Watchlist watchlistModel)
		{
			await _context.Watchlist.AddAsync(watchlistModel);
			await _context.SaveChangesAsync();
			return watchlistModel;
		}

		public async Task<Watchlist?> DeleteWatchlistAsync(int Id)
		{
			var watchlistModel = await _context.Watchlist.FirstOrDefaultAsync(x => x.Id == Id);

			if (watchlistModel == null)
			{
				return null;
			}

			// Remove WatchList
			_context.Watchlist.Remove(watchlistModel);
			await _context.SaveChangesAsync();
			return watchlistModel;
		}

		public async Task<List<Watchlist>> GetAllAsync()
		{

			return await _context.Watchlist.Include(wl => wl.Items).ToListAsync();

		}

		public async Task<Watchlist?> GetByIdAsync(int Id)
		{
			return await _context.Watchlist.FindAsync(Id);
		}
	}
}
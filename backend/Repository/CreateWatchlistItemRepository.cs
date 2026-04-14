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
	public class CreateWatchlistItemRepository : IWatchlistItemRepository
	{
		private readonly ApplicationDbContext _context;
		public CreateWatchlistItemRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<WatchlistItems> CreateWatchlistAsync(WatchlistItems watchlistModel)
		{
			await _context.WatchlistItems.AddAsync(watchlistModel);
			await _context.SaveChangesAsync();
			return watchlistModel;
		}

		public async Task<WatchlistItems?> DeleteWatchlistAsync(int id)
		{
			var watchlistItem = await _context.WatchlistItems.FirstOrDefaultAsync(x => x.Id == id);

			if (watchlistItem == null)
				return null;

			_context.WatchlistItems.Remove(watchlistItem);
			await _context.SaveChangesAsync();

			return watchlistItem;
		}


		public async Task<WatchlistItems?> GetByIdAsync(int Id, int watchlistId)
		{
			return await _context.WatchlistItems
					  .FirstOrDefaultAsync(i => i.Id == Id && i.WatchlistId == watchlistId);
		}

		public async Task<bool> ExistsAsync(int watchlistId, string baseCurrency, string quoteCurrency)
		{
			return await _context.WatchlistItems
				.AnyAsync(x => x.WatchlistId == watchlistId
							&& x.BaseCurrency == baseCurrency
							&& x.QuoteCurrency == quoteCurrency);
		}
	}
}
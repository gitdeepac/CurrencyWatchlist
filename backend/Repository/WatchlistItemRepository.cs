using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.WatchlistItem;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
	public class WatchlistItemRepository : IWatchlistItemRepository
	{
		private readonly ApplicationDbContext _context;
		public WatchlistItemRepository(ApplicationDbContext context)
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


		public async Task<WatchlistItemDto?> GetByIdAsync(int Id, int watchlistId)
		{
			var item = await _context.WatchlistItems
					  .FirstOrDefaultAsync(i => i.Id == Id && i.WatchlistId == watchlistId);
			if (item == null)
				return null;


			var result = new WatchlistItemDto
			{
				Id = item.Id,
				WatchlistId = item.WatchlistId,
				BaseCurrency = item.BaseCurrency,
				QuoteCurrency = item.QuoteCurrency,
			};
			return result;
		}

		public async Task<bool> ExistsAsync(int watchlistId, string baseCurrency, string quoteCurrency)
		{
			return await _context.WatchlistItems
				.AnyAsync(x => x.WatchlistId == watchlistId
							&& x.BaseCurrency == baseCurrency
							&& x.QuoteCurrency == quoteCurrency);
		}

		public async Task<List<WatchlistItemDto>> GetAllAsync(int watchlistId)
		{

			//1. Get Watch list Items
			//2. Get Distinct Currency List
			//3. Get Rate through Currency 
			//4. Return result.
			var items = await _context.WatchlistItems.Where(x => x.WatchlistId == watchlistId).ToListAsync();
			var currencies = items.Select(x => x.QuoteCurrency).Distinct().ToList();
			var rates = await _context.RateSnapShot.Where(r => currencies.Contains(r.QuoteCurrency)).ToListAsync();

			var result = items.Select(item =>
			{
				var rate = rates.FirstOrDefault(r =>
					r.QuoteCurrency == item.QuoteCurrency);

				return new WatchlistItemDto
				{
					Id = item.Id,
					WatchlistId = item.WatchlistId,
					BaseCurrency = item.BaseCurrency,
					QuoteCurrency = item.QuoteCurrency,
					LatestRate = rate?.Rate
				};
			}).ToList();

			return result;
		}
	}
}
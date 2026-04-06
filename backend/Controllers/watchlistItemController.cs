using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.WatchlistItem;
using backend.Helpers;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	[Route("api/watchlists/{watchlistId:int}/items")]
	[ApiController]
	public class WatchlistsItemController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public WatchlistsItemController(ApplicationDbContext applicationDbContext)
		{
			_context = applicationDbContext;
		}

		[HttpGet("{Id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int watchlistId, [FromRoute] int Id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlistExists = await _context.Watchlist.AnyAsync(w => w.Id == watchlistId);

			if (!watchlistExists)
			{
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist with ID {watchlistId} does not exist."));
			}

			var watchlistItem = await _context.WatchlistItems
					  .FirstOrDefaultAsync(i => i.Id == Id && i.WatchlistId == watchlistId);

			if (watchlistItem == null)
				return NotFound(ApiResponse<object?>.NotFound($"Item with ID {Id} was not found in watchlist {watchlistId}"));

			return Ok(ApiResponse<object?>.Success(watchlistItem.ToWatchlistItemDto(), "Successfully Fetch all WatchlistItems", 200));
		}

		[HttpPost]
		public async Task<IActionResult> Create(
		  [FromRoute] int watchlistId,
		  [FromBody] CreateWatchlistItemRequestDto watchlistItemDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);


			var watchlistExists = await _context.Watchlist.AnyAsync(w => w.Id == watchlistId);
			if (!watchlistExists)
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist with ID {watchlistId} does not exist."));

			var itemExists = await _context.WatchlistItems
			  .AnyAsync(x => x.WatchlistId == watchlistId
						  && x.BaseCurrency == watchlistItemDto.BaseCurrency && x.QuoteCurrency == watchlistItemDto.QuoteCurrency);

			if (itemExists)
				return Ok(ApiResponse<object?>.Error(null, "This item already exists in the watchlist.", 200));


			var watchlistItem = watchlistItemDto.ToWatchlistItemFromCreateDTO(watchlistId);

			await _context.WatchlistItems.AddAsync(watchlistItem);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetById), new { watchlistId, id = watchlistItem.Id },
			ApiResponse<object?>.Success(watchlistItem.ToWatchlistItemDto(), "Successfully Created Wathclist Item.", 201));
		}

		[HttpDelete]
		[Route("{Id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int Id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlistItemModel = await _context.WatchlistItems.FirstOrDefaultAsync(x => x.Id == Id);

			if (watchlistItemModel == null)
			{
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist Item with ID {Id} does not exist."));
			}

			_context.WatchlistItems.Remove(watchlistItemModel);
			await _context.SaveChangesAsync();

			return Ok(ApiResponse<object?>.Success(null, $"Successfully Deleted Watchlist Item with ID {Id}", 200));
		}
	}
}
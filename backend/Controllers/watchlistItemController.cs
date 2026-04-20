using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.WatchlistItem;
using backend.Helpers;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	// Manages items within a specific watchlist — nested under /api/watchlists/{watchlistId}/items.
	[Route("api/Watchlists/{watchlistId:int}/items")]
	[ApiController]
	public class WatchlistsItemController : ControllerBase
	{

		private readonly IWatchlistItemRepository _watchlistItemRepository;
		private readonly IWatchlistRepository _watchlistRepository;
		public WatchlistsItemController(IWatchlistItemRepository watchlistItemRepository, IWatchlistRepository watchlistRepository)
		{
			_watchlistItemRepository = watchlistItemRepository;
			_watchlistRepository = watchlistRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromRoute] int watchlistId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlistExist = await _watchlistRepository.GetByIdAsync(watchlistId);

			if (watchlistExist == null)
			{
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist with ID {watchlistId} does not exist."));
			}

			var watchlistItem = await _watchlistItemRepository.GetAllAsync(watchlistId);

			if (watchlistItem == null)
				return NotFound(ApiResponse<object?>.NotFound($"Item was not found in watchlist {watchlistId}"));

			var watchlistDto = watchlistItem.Select(wl => wl);
			return Ok(ApiResponse<object?>.Success(watchlistDto, "Successfully Fetch all WatchlistItems", 200));
		}


		// Returns a single items
		[HttpGet("{Id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int watchlistId, [FromRoute] int Id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			//var watchlistExists = await _context.Watchlist.AnyAsync(w => w.Id == watchlistId);
			var watchlistExist = await _watchlistRepository.GetByIdAsync(watchlistId);


			if (watchlistExist == null)
			{
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist with ID {watchlistId} does not exist."));
			}

			var watchlistItem = await _watchlistItemRepository.GetByIdAsync(Id, watchlistId);

			if (watchlistItem == null)
				return NotFound(ApiResponse<object?>.NotFound($"Item with ID {Id} was not found in watchlist {watchlistId}"));

			return Ok(ApiResponse<object?>.Success(watchlistItem, "Successfully Fetch WatchlistItems", 200));
		}

		// Adds a currency pair item to a watchlist
		[HttpPost]
		public async Task<IActionResult> Create(
				[FromRoute] int watchlistId,
				[FromBody] CreateWatchlistItemRequestDto watchlistItemDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);


			var watchlistExists = await _watchlistRepository.GetByIdAsync(watchlistId);
			if (watchlistExists == null)
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist with ID {watchlistId} does not exist."));

			// Duplicate check — same pair in same watchlist is not allowed
			var itemExists = await _watchlistItemRepository.ExistsAsync(watchlistId, watchlistItemDto.BaseCurrency, watchlistItemDto.QuoteCurrency);

			if (itemExists)
				return Ok(ApiResponse<object?>.Error(null, "This item already exists in the watchlist.", 200));


			var watchlistItem = watchlistItemDto.ToWatchlistItemFromCreateDTO(watchlistId);

			await _watchlistItemRepository.CreateWatchlistAsync(watchlistItem);

			return CreatedAtAction(nameof(GetById), new { watchlistId, id = watchlistItem.Id },
			ApiResponse<object?>.Success(watchlistItem.ToWatchlistItemDto(), "Successfully Created Watchlist Item.", 201));
		}

		// Hard delete by item ID
		[HttpDelete]
		[Route("{Id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int watchlistId, [FromRoute] int Id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlistExists= await _watchlistRepository.GetByIdAsync(watchlistId);
			if (watchlistExists == null)
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist with ID {watchlistId} does not exist."));

			var deletedItem = await _watchlistItemRepository.DeleteWatchlistAsync(Id);

			if (deletedItem == null)
			{
				return NotFound(ApiResponse<object?>.NotFound($"Watchlist Item with ID {Id} does not exist."));
			}

			return Ok(ApiResponse<object?>.Success(null, $"Successfully Deleted Watchlist Item with ID {Id}", 200));
		}
	}
}
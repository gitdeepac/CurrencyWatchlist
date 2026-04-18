using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Watchlist;
using backend.Helpers;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{

	// Manages watchlists and their associated items.

	[Route("api/[controller]")]
	[ApiController]
	public class WatchlistsController : ControllerBase
	{
		// DB context injected via DI
		private readonly IWatchlistRepository _watchlistRepository;
		public WatchlistsController(IWatchlistRepository watchlistRepository)
		{
			_watchlistRepository = watchlistRepository;
		}

		// Returns all watchlists with items
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlists = await _watchlistRepository.GetAllAsync();

			var watchlistDto = watchlists.Select(wl => wl.ToWatchlistDto());

			return Ok(ApiResponse<object>.Success(
				watchlistDto,
				"Successfully fetch list",
				200
			));
		}


		// Returns a single watchlist by ID — items not included here, intentional
		[HttpGet("{Id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			var watchlist = await _watchlistRepository.GetByIdAsync(id);

			if (watchlist == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}

			return Ok(ApiResponse<object?>.Success(watchlist, "Successfully Fetch Watchlist", 200));
		}

		// Creates a new watchlist
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateWatchlistRequestDto watchlistDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlistModel = watchlistDto.ToWatchlistFromCreateDTO();
			await _watchlistRepository.CreateWatchlistAsync(watchlistModel);

			return CreatedAtAction(nameof(GetById), new { id = watchlistModel.Id },
				ApiResponse<object?>.Success(watchlistModel.ToWatchlistDto(), "Successfully created watchlist", 201));
		}


		// Hard delete — no soft delete, intentional
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int Id)
		{
			var watchlistModel = await _watchlistRepository.DeleteWatchlistAsync(Id);

			if (watchlistModel == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}

			return Ok(ApiResponse<object?>.Success(null, $"Successfully Deleted Watchlist {Id}", 200));
		}
	}
}
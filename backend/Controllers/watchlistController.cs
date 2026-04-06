using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Watchlist;
using backend.Helpers;
using backend.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class WatchlistsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public WatchlistsController(ApplicationDbContext applicationDbContext)
		{
			_context = applicationDbContext;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlists = await _context.Watchlist.Include(wl => wl.Items).ToListAsync();

			var watchlistDto = watchlists.Select(wl => wl.ToWatchlistDto());

			return Ok(ApiResponse<object?>.Success(
				data: watchlistDto,
				message: "Successfully fetch list",
				statusCode: 200
			));
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			var watchlist = await _context.Watchlist.FindAsync(id);

			if (watchlist == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}

			return Ok(ApiResponse<object?>.Success(watchlist, "Successfully Fetch Watchlist", 200));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateWatchlistRequestDto watchlistDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlistModel = watchlistDto.ToWatchlistFromCreateDTO();
			await _context.Watchlist.AddAsync(watchlistModel);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetById), new { id = watchlistModel.Id },
				ApiResponse<object?>.Success(watchlistModel.ToWatchlistDto(), "Successfully created watchlist", 201));
		}

		[HttpDelete]
		[Route("{Id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int Id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var watchlistModel = await _context.Watchlist.FirstOrDefaultAsync(x => x.Id == Id);

			if (watchlistModel == null)
			{
				return NotFound(ApiResponse<object?>.NotFound("No record found"));
			}

			_context.Watchlist.Remove(watchlistModel);
			await _context.SaveChangesAsync();

			return Ok(ApiResponse<object?>.Success(null, $"Successfully Deleted Watchlist {Id}", 200));
		}
	}
}
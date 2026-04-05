using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.WatchlistItem;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
  [Route("api/watchlists/{watchlistId:int}/items")]
  [ApiController]
  public class watchlistsItemController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    public watchlistsItemController(ApplicationDbContext applicationDbContext)
    {
      _context = applicationDbContext;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int watchlistId, [FromRoute] int id)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var watchlistExists = await _context.Watchlist.AnyAsync(w => w.Id == watchlistId);

      if (!watchlistExists)
      {
        return NotFound(new { message = $"Watchlist with ID {watchlistId} does not exist." });
      }

      var watchlistItem = await _context.WatchlistItems
                .FirstOrDefaultAsync(i => i.Id == id && i.WatchlistId == watchlistId);

      if (watchlistItem == null)
        return NotFound(new { message = $"Item with ID {id} was not found in watchlist {watchlistId}." });

      return Ok(watchlistItem.ToWatchlistItemDto());
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
        return NotFound(new { message = $"Watchlist with ID {watchlistId} does not exist." });

      var itemExists = await _context.WatchlistItems
        .AnyAsync(x => x.WatchlistId == watchlistId 
                    && x.BaseCurrency == watchlistItemDto.BaseCurrency && x.QuoteCurrency == watchlistItemDto.QuoteCurrency);

    if (itemExists)
        return Conflict(new { message = "This item already exists in the watchlist." });


      var watchlistItem = watchlistItemDto.ToWatchlistItemFromCreateDTO(watchlistId);

      await _context.WatchlistItems.AddAsync(watchlistItem);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetById), new { watchlistId, id = watchlistItem.Id }, watchlistItem.ToWatchlistItemDto());
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
        return NotFound();
      }

      _context.WatchlistItems.Remove(watchlistItemModel);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
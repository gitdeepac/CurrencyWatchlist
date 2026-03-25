using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Watchlist;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class watchlistsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    public watchlistsController(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if(!ModelState.IsValid)
          return BadRequest(ModelState);

        var watchlists = await _context.Watchlist.ToListAsync();

        var watchlistDto = watchlists.Select(wl => wl.ToWatchlistDto());

        return Ok(watchlistDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var watchlist = await _context.Watchlist.FindAsync(id);
      
      if(watchlist == null)
      {
          return NotFound();
      }
      
      return Ok(watchlist.ToWatchlistDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWatchlistRequestDto watchlistDto)
    {
       if(!ModelState.IsValid)
          return BadRequest(ModelState);

      var watchlistModel = watchlistDto.ToWatchlistFromCreateDTO();
      await _context.Watchlist.AddAsync(watchlistModel);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetById), new { id = watchlistModel.Id}, watchlistModel.ToWatchlistDto());
    }

    [HttpDelete]
    [Route("{Id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int Id)
    {
       if(!ModelState.IsValid)
          return BadRequest(ModelState);

      var watchlistModel = await _context.Watchlist.FirstOrDefaultAsync(x => x.Id == Id);

      if(watchlistModel == null)
      {
        return NotFound();
      }

      _context.Watchlist.Remove(watchlistModel);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
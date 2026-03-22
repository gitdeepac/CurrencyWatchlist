using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class watchlistController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    public watchlistController(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var watchlists = _context.Watchlist.ToList(); // Differed Execuation
        return Ok(watchlists);
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var watchlist = _context.Watchlist.Find(id);
      
      if(watchlist == null)
      {
          return NotFound();
      }
      
      return Ok(watchlist);
    }
  }
}
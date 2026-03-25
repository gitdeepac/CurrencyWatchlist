using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
          
        }

        public DbSet<Watchlist> Watchlist { get; set; }
        public DbSet<WatchlistItems> WatchlistItems { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class WatchlistItems
    {
        public int Id { get; set; }
        public int? WatchlistId { get; set; }
        // Navigation
        public Watchlist? Watchlist { get; set; }
        // relationship to Watchlist Model
        [Column(TypeName = "char(3)")]
        public string BaseCurrency { get; set; } = String.Empty;
        [Column(TypeName = "char(3)")]
        public string QuoteCurrency { get; set; } = String.Empty;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        
    }
}
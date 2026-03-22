using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Watchlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
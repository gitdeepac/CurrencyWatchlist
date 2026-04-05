using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class RateSnapShot
    {
        public int Id { get; set; }

        public String BaseCurrency { get; set; } = String.Empty;
        public String QuoteCurrency { get; set; } = String.Empty;
        public Decimal Rate { get; set; } 
        public DateTime SourceTimestamp { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
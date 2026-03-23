using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Watchlist
{
    public class CreateWatchlistRequestDto
    {
      [Required]
      [MinLength(5, ErrorMessage = "Name must be 5 Characters")]
      [MaxLength(10, ErrorMessage = "Name cannot be over 10 Characters")]
      public string Name { get; set; } = string.Empty;
      public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
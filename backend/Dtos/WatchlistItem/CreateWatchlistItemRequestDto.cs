using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace backend.Dtos.WatchlistItem
{
    public class CreateWatchlistItemRequestDto
    {
      [Required(ErrorMessage = "Watchlist Id is required")]
      public int WatchlistId { get; set; }

      [Required(ErrorMessage = "Base Currency is required")]
      [StringLength(3, ErrorMessage = "Currency cannot exceed 3 characters.")]
      public string BaseCurrency { get; set; } = string.Empty;

      [Required(ErrorMessage = "Quote Currency is required")]
      [StringLength(3, ErrorMessage = "Currency cannot exceed 3 characters.")]
      public string QuoteCurrency { get; set; } = string.Empty;
    }
}
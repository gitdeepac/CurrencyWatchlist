using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.External;
using backend.Dtos.Rate;
using backend.Dtos.WatchlistItem;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ratesController : Controller
   {
      // Db context
      private readonly ApplicationDbContext _context;
      private readonly IHttpClientFactory _httpClientFactory;
      private readonly ILogger<ratesController> _logger;



      public ratesController(ApplicationDbContext applicationDbContext, IHttpClientFactory httpClientFactory, ILogger<ratesController> logger)
      {
         _context = applicationDbContext;
         _httpClientFactory = httpClientFactory;
         _logger = logger;
      }

      [HttpPost("refresh")]
      public async Task<IActionResult> RefreshRate()
      {

         var httpClient = _httpClientFactory.CreateClient();
         var semaphore = new SemaphoreSlim(3, 3);
         // 1. Get unique combinations from SQLite
         var distinctPairs = await _context.WatchlistItems
         .Select(w_items => new watchlistItemSummaryDto
         {
            BaseCurrency = w_items.BaseCurrency,
            QuoteCurrency = w_items.QuoteCurrency
         })
               .Distinct()
               .ToListAsync();

         // 2. Map each pair to a Task (Parallel Requests)
         var fetchTasks = distinctPairs.Select(async pair =>
         {

            await semaphore.WaitAsync();

            try
            {
               // Replace this URL with your actual Exchange Rate API endpoint
               // https://api.frankfurter.app/latest?from=USD&to=AUD'
               var url = $"https://api.frankfurter.app/latest?from={pair.BaseCurrency}&to={pair.QuoteCurrency}";

               var apiResponse = await httpClient.GetFromJsonAsync<ExternalApiRawResponse>(url);

               var rateValue = apiResponse?.Rates.FirstOrDefault().Value;

               if (apiResponse != null && rateValue != null)
               {
                  return new RateSnapShotDto
                  {
                     BaseCurrency = apiResponse.Base,
                     QuoteCurrency = apiResponse.Rates.Keys.FirstOrDefault() ?? "",
                     Rate = (decimal)rateValue,
                     SourceTimestamp = DateTime.Parse(apiResponse.Date),
                     FetchedAt = DateTime.Now
                  };
               }
               return null; // pair failed or returned no data
            }
            catch (HttpRequestException ex)
            {
               // Network error, timeout, Frankfurter returned non-2xx
               // Log it so you can see which pair failed and why
               _logger.LogWarning("Network error for {Base}/{Quote}: {Error}", pair.BaseCurrency, pair.QuoteCurrency, ex.Message);
               return null;
            }
            catch (Exception ex)
            {
               // Anything else unexpected — JSON parse error, etc.
               _logger.LogError("Unexpected error for {Base}/{Quote}: {Error}", pair.BaseCurrency, pair.QuoteCurrency, ex.Message);
               return null;
            }
            finally
            {
               semaphore.Release();
            }
         });

         var results = await Task.WhenAll(fetchTasks);
         // Filter out any nulls (failed pairs)
         var fetchedRates = results
               .Where(r => r != null)
               .Cast<RateSnapShotDto>()
               .ToList();

         var snapshots = fetchedRates.Select(dto => new RateSnapShot
         {
            BaseCurrency = dto.BaseCurrency,
            QuoteCurrency = dto.QuoteCurrency,
            Rate = dto.Rate,
            SourceTimestamp = dto.SourceTimestamp,
            CreateAt = DateTime.Now
         }).ToList();

         try
         {
            await _context.AddRangeAsync(snapshots);
            await _context.SaveChangesAsync();
         }
         catch (Exception ex)
         {
            _logger.LogError("Database save failed: {Error}", ex.Message);

            return StatusCode(500, new
            {
               message = "Rates were fetched but could not be saved.",
               saved = 0
            });
         }


         return Accepted(new
         {
            message = "Rates refreshed successfully.",
            saved = snapshots.Count
         });
      }
   }
}
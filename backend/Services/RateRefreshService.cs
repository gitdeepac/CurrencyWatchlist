using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.External;
using backend.Dtos.Rate;
using backend.Dtos.WatchlistItem;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	public class RateRefreshService
	{
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<RateRefreshService> _logger;

		public RateRefreshService(ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory, ILogger<RateRefreshService> logger)
		{
			_applicationDbContext = dbContext;
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task<RateRefreshResult> RefreshRateAsync()
		{
			// create http client object
			var httpClient = _httpClientFactory.CreateClient();

			// Semaphore — max 3 concurrent requests to Frankfurter
			var semaphore = new SemaphoreSlim(3, 3);

			var distinctPairs = await _applicationDbContext.WatchlistItems
			.Select(w => new WatchlistItemSummaryDto
			{
				BaseCurrency = w.BaseCurrency,
				QuoteCurrency = w.QuoteCurrency
			})
			.Distinct()
			.ToListAsync();

			if (distinctPairs.Count == 0)
			{
				_logger.LogInformation("No pairs found in watchlist — nothing to refresh.");
				return new RateRefreshResult { Message = "Watchlist is empty." };
			}

			var fetchTasks = distinctPairs.Select(async pair =>
			{
				await semaphore.WaitAsync();

				try
				{
					var url = $"https://api.frankfurter.app/latest?from={pair.BaseCurrency}&to={pair.QuoteCurrency}";

					var apiResponse = await httpClient.GetFromJsonAsync<ExternalApiRawResponse>(url);

					var rateValue = apiResponse?.Rate;

					if (apiResponse != null && rateValue != null)
					{
						return new RateSnapShotDto
						{
							BaseCurrency = apiResponse.Base,
							QuoteCurrency = apiResponse.Quote ?? "",
							Rate = (decimal)rateValue,
							SourceTimestamp = apiResponse.Date.ToDateTime(TimeOnly.MinValue),
							FetchedAt = DateTime.Now
						};
					}
					return null;
				}
				catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					_logger.LogWarning("Invalid currency pair {Base}/{Quote}", pair.BaseCurrency, pair.QuoteCurrency);
					return null;
				}
				catch (HttpRequestException ex)
				{
					_logger.LogWarning("Network error for {Base}/{Quote}: {Error}", pair.BaseCurrency, pair.QuoteCurrency, ex.Message);
					return null;
				}
				catch (Exception ex)
				{

					_logger.LogError("Unexpected error for {Base}/{Quote}: {Error}", pair.BaseCurrency, pair.QuoteCurrency, ex.Message);
					return null;
				}
				finally
				{
					semaphore.Release();
				}
			});


			var results = await Task.WhenAll(fetchTasks);

			// Filter Empty ones.
			var fetchedRates = results
				  .Where(r => r != null)
				  .Cast<RateSnapShotDto>()
				  .ToList();

			int created = 0;
			int updated = 0;

			foreach (var dto in fetchedRates)
			{
				var existing = await _applicationDbContext.RateSnapShot
					.FirstOrDefaultAsync(r =>
						r.BaseCurrency == dto.BaseCurrency &&
						r.QuoteCurrency == dto.QuoteCurrency);

				if (existing == null) // create case
				{

					await _applicationDbContext.RateSnapShot.AddAsync(new RateSnapShot
					{
						BaseCurrency = dto.BaseCurrency,
						QuoteCurrency = dto.QuoteCurrency,
						Rate = dto.Rate,
						SourceTimestamp = dto.SourceTimestamp,
						CreateAt = DateTime.Now
					});
					created++;
				}
				else // update case
				{
					existing.Rate = dto.Rate;
					existing.SourceTimestamp = dto.SourceTimestamp;
					existing.CreateAt = DateTime.Now;
					updated++;
				}
			}

			try
			{
				await _applicationDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Database save failed: {Error}", ex.Message);

				return new RateRefreshResult
				{
					Success = false,
					Message = "Rates were fetched but could not be saved."
				};
			}
			_logger.LogInformation("Refresh complete. Created={Created} Updated={Updated}", created, updated);

			return new RateRefreshResult
			{
				Success = true,
				Message = "Rates refreshed successfully.",
				TotalCreatedRecords = created,
				TotalUpdatedRecords = updated
			};
		}

		// Internal result model 
		public class RateRefreshResult
		{
			public bool Success { get; set; } = true;
			public String Message { get; set; } = string.Empty;
			public int TotalCreatedRecords { get; set; } = 0;
			public int TotalUpdatedRecords { get; set; } = 0;
		}
	}
}
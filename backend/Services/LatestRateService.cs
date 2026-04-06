using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.External;
using backend.Models;

namespace backend.Services
{
	public class LatestRateService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<LatestRateService> _logger;

		public LatestRateService(IHttpClientFactory httpClientFactory, ILogger<LatestRateService> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task<LiveRateResult?> GetLatestRateAsync(string baseCurrency, string quoteCurrency)
		{
			var baseCurr = baseCurrency.Trim().ToUpper();
			var quoteCurr = quoteCurrency.Trim().ToUpper();

			_logger.LogInformation("Fetching latest rate for {Base}/{Quote}", baseCurr, quoteCurr);
			_logger.LogInformation("https://api.frankfurter.dev/v2/rates?base={baseCurr}&quotes={quoteCurr}", baseCurr, quoteCurr);

			var httpClient = _httpClientFactory.CreateClient();
			var url = $"https://api.frankfurter.dev/v2/rates?base={baseCurr}&quotes={quoteCurr}";

			try
			{
				var apiResponse = await httpClient.GetFromJsonAsync<List<ExternalApiRawResponse>>(url);


				if (apiResponse != null && apiResponse.Count != 0)
				{
					var item = apiResponse.First();
					return new LiveRateResult
					{
						BaseCurrency = item.Base,
						QuoteCurrency = item.Quote,
						Rate = item.Rate,
						RateDate = item.Date
					};
				}
				else
				{
					_logger.LogWarning("Empty response from Frankfurter for {Base}/{Quote}", baseCurr, quoteCurr);
					return null;
				}

			}
			catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				_logger.LogWarning("Invalid currency pair {Base}/{Quote}", baseCurr, quoteCurr);
				return null;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError("Network error fetching live rate for {Base}/{Quote}: {Error}", baseCurr, quoteCurr, ex.Message);
				return null;
			}
			catch (Exception ex)
			{
				_logger.LogError("Unexpected error fetching live rate for {Base}/{Quote}: {Error}", baseCurr, quoteCurr, ex.Message);
				return null;
			}
		}
	}
	public class LiveRateResult
	{
		public string BaseCurrency { get; set; } = string.Empty;
		public string QuoteCurrency { get; set; } = string.Empty;
		public decimal Rate { get; set; }
		public DateOnly RateDate { get; set; }
	}
}
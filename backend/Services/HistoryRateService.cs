using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.External;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Services
{
	public class HistoryRateService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<HistoryRateService> _logger;

		public HistoryRateService(IHttpClientFactory httpClientFactory, ILogger<HistoryRateService> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task<List<HistoryRateResult>> GetHistoryRateAsync(string baseCurrency, string quoteCurrency, DateOnly fromDate, DateOnly toDate)
		{
			var baseCurr = baseCurrency.Trim().ToUpper();
			var quoteCurr = quoteCurrency.Trim().ToUpper();
			var startDate = fromDate;
			var endDate = toDate;

			_logger.LogInformation("Fetching rate for {Base}/{Quote} history data {From}/{To} based on from and to date", baseCurr, quoteCurr, startDate, endDate);

			var httpClient = _httpClientFactory.CreateClient();
			var url = $"https://api.frankfurter.dev/v2/rates?base={baseCurr}&quotes={quoteCurr}&from={startDate}$to={endDate}";

			try
			{
				var apiResponse = await httpClient.GetFromJsonAsync<List<ExternalApiRawResponse>>(url);


				if (apiResponse != null && apiResponse.Count != 0)
				{
					return apiResponse.Select(item => new HistoryRateResult
					{
						BaseCurrency = item.Base,
						QuoteCurrency = item.Quote,
						Rate = item.Rate,
						RateDate = item.Date
					}).ToList();
				}
				else
				{
					_logger.LogWarning("Empty response from Frankfurter for {Base}/{Quote}", baseCurr, quoteCurr);
					return new List<HistoryRateResult>();
				}

			}
			catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				_logger.LogWarning("Invalid currency pair {Base}/{Quote}", baseCurr, quoteCurr);
				return new List<HistoryRateResult>();
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError("Network error fetching live rate for {Base}/{Quote}: {Error}", baseCurr, quoteCurr, ex.Message);
				return new List<HistoryRateResult>();
			}
			catch (Exception ex)
			{
				_logger.LogError("Unexpected error fetching live rate for {Base}/{Quote}: {Error}", baseCurr, quoteCurr, ex.Message);
				return new List<HistoryRateResult>();
			}
		}
	}
	public class HistoryRateResult
	{
		public string BaseCurrency { get; set; } = string.Empty;
		public string QuoteCurrency { get; set; } = string.Empty;
		public decimal Rate { get; set; }
		public DateOnly RateDate { get; set; }
	}
}
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

using PriceTracker.Models;
using PriceTracker.Settings;

namespace PriceTracker.Services
{
    public class ApiService
    {
        private readonly ILogger<ApiService> _logger;

        private readonly ApiSettings _settings;
        
        private readonly HttpClient _httpClient;

        public ApiService(IOptions<ApiSettings> settings, ILogger<ApiService> logger)
        {
            _logger = logger;
            _settings = settings.Value;
            _httpClient = new HttpClient();
        }

        public async Task<HistoricalDataPrice?> GetTickerPrice(string ticker, int timestamp)
        {
            string uri = string.Format("{0}/quote/{1}?range=1d&interval=1m", _settings.Url, ticker);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("No data available at the moment");
                }

                string responseString = await response.Content.ReadAsStringAsync();

                JObject json = JObject.Parse(responseString);

                List<JToken> data = json.SelectTokens(
                    String.Format("$.results[0].historicalDataPrice[?(@.date > {0})]", timestamp)
                ).ToList();

                if (data.Any())
                {
                    return data.Last().ToObject<HistoricalDataPrice>();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to fetch ticker price. Exception: {e}", e.ToString());
            }
            
            return null;
        }

        public async Task<bool> ValidateTicker(string ticker)
        {
            string uri = string.Format("{0}/available?search={1}", _settings.Url, ticker);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                string responseString = await response.Content.ReadAsStringAsync();

                JObject responseJson = JObject.Parse(responseString);

                return responseJson["stocks"].Values().Contains(ticker);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to validate ticker. Exception: {e}", e.ToString());
            }

            return false;
        }
    }
}

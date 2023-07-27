using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

using PriceTracker.Configuration;

namespace PriceTracker.Services
{
    public class ApiService
    {
        private readonly ILogger<ApiService> _logger;

        private readonly HttpClient _httpClient;
        private readonly string? _apiUrl;

        public ApiService(IOptions<Api> options, ILogger<ApiService> logger)
        {
            _logger = logger;
            _apiUrl = options.Value.Url;
            _httpClient = new HttpClient();
        }

        public async Task GetStockData(string ticker)
        {
            string uri = string.Format("{0}/quote/{1}?range=1d&interval=1d", _apiUrl, ticker);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                string responseString = await response.Content.ReadAsStringAsync();

                JObject responseJson = JObject.Parse(responseString);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError(responseJson.SelectToken("error").ToString());
                }

                JToken data = responseJson.SelectToken("$.results[0].historicalDataPrice[0]");

                _logger.LogInformation(data.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}

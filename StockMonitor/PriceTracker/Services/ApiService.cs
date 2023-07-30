using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

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

        public async Task GetTickerData(string ticker)
        {
            string uri = string.Format("{0}/quote/{1}?range=1d&interval=1d", _settings.Url, ticker);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("No data available at the moment");
                }

                string responseString = await response.Content.ReadAsStringAsync();

                JObject responseJson = JObject.Parse(responseString);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError(responseJson.SelectToken("error").ToString());
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to fetch ticker price. Exception: {e}", e.ToString());
            }
            
            return null;
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

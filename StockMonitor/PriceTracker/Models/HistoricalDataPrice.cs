using Newtonsoft.Json;

namespace PriceTracker.Models
{
    public class HistoricalDataPrice
    {
        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("open")]
        public decimal? Open { get; set; }

        [JsonProperty("high")]
        public decimal? High { get; set; }

        [JsonProperty("close")]
        public decimal? Close { get; set; }

        [JsonProperty("volume")]
        public int? Volume { get; set; }

        [JsonProperty("adjustedClose")]
        public decimal? AjustedClose { get; set; }
    }
}

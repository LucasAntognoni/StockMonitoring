namespace PriceTracker.Configuration
{
    public class Smtp
    {
        public string? Host { get; set; }

        public int Port { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public List<string>? Emails { get; set; }
    }
}

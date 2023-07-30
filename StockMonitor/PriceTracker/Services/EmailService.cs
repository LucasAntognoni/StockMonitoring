using Microsoft.Extensions.Options;

using MimeKit;
using MailKit.Net.Smtp;

using PriceTracker.Settings;

namespace PriceTracker.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        
        private readonly SmtpSettings _settings;

        private readonly EmailSettings _sender;

        private readonly List<EmailSettings> _recipients;

        public EmailService(IOptions<SmtpSettings> settings, IOptions<EmailSettings> sender, IOptions<List<EmailSettings>> recipients, ILogger<EmailService> logger)
        {
            _logger     = logger;
            _settings   = settings.Value;
            _sender     = sender.Value;
            _recipients = recipients.Value;
        }

        public async Task SendEmail(string operation, string ticker, decimal referencePrice, decimal currentPrice)
        {
            try
            {
                MimeMessage email = new MimeMessage();

                email.From.Add (new MailboxAddress(_sender.Name, _sender.Address));
                
                foreach (EmailSettings recipient in _recipients) {
                    email.To.Add(new MailboxAddress(recipient.Name, recipient.Address));
                }

                email.Subject = string.Format("[{0}] [{1}] price alert!", operation, ticker);

                BodyBuilder emailBodyBuilder = new BodyBuilder();

                emailBodyBuilder.TextBody = string.Format(
                    "{0} has reached the desired price.\n\n-Reference price: R${1}\n\n-Current price: R${2}",
                    ticker,
                    referencePrice,
                    currentPrice
                );
                
                email.Body = emailBodyBuilder.ToMessageBody();                

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.Auto);
                    await smtpClient.AuthenticateAsync(_settings.UserName, _settings.Password);
                    await smtpClient.SendAsync(email);
                    await smtpClient.DisconnectAsync(true);
                }

                _logger.LogInformation("E-mail sent!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send e-mail. Exception: {ExceptionMessage}", ex.Message);
            }
        }
    }
}

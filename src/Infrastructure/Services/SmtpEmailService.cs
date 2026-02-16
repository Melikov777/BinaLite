using System.Net;
using System.Net.Mail;
using Application.Abstracts.Services;
using Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _options;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailOptions> options, ILogger<SmtpEmailService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string htmlBody, string? plainTextBody = null, CancellationToken ct = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogWarning("Email sending is disabled. Skipping email to {To}", to);
            return;
        }

        if (string.IsNullOrWhiteSpace(to))
            return;

        using var client = new SmtpClient(_options.Smtp.Host, _options.Smtp.Port)
        {
            EnableSsl = _options.Smtp.UseSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        if (!string.IsNullOrWhiteSpace(_options.Smtp.Username))
            client.Credentials = new NetworkCredential(_options.Smtp.Username, _options.Smtp.Password);

        var from = new MailAddress(_options.Sender.Email, _options.Sender.Name);
        using var message = new MailMessage(from, new MailAddress(to))
        {
            Subject = subject
        };

        if (!string.IsNullOrWhiteSpace(htmlBody))
        {
            message.Body = htmlBody;
            message.IsBodyHtml = true;
        }
        else
        {
            message.Body = plainTextBody ?? string.Empty;
            message.IsBodyHtml = false;
        }

        await client.SendMailAsync(message, ct);
        _logger.LogInformation("Email sent to {To} with subject '{Subject}'", to, subject);
    }
}

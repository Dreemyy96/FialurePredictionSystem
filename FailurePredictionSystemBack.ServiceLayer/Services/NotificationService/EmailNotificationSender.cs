using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.ConfigModels;
using FailurePredictionSystemBack.Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FailurePredictionSystemBack.ServiceLayer.Services.NotificationService;

public class EmailNotificationSender : IEmailNotificationSender
{
    private readonly EmailSettings _settings;

    public EmailNotificationSender(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAsync(
        Notification notification,
        string recipientEmail,
        CancellationToken cancellationToken)
    {
        if (!_settings.Enabled)
            return;

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _settings.FromName,
            _settings.FromEmail));

        message.To.Add(MailboxAddress.Parse(recipientEmail));

        message.Subject = notification.Subject;

        message.Body = new TextPart("plain")
        {
            Text = notification.Message
        };

        using var client = new SmtpClient();

        var secureSocketOptions = _settings.UseSsl
            ? SecureSocketOptions.StartTls
            : SecureSocketOptions.Auto;

        await client.ConnectAsync(
            _settings.SmtpHost,
            _settings.SmtpPort,
            secureSocketOptions,
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(_settings.UserName))
        {
            await client.AuthenticateAsync(
                _settings.UserName,
                _settings.Password,
                cancellationToken);
        }

        await client.SendAsync(
            message,
            cancellationToken);

        await client.DisconnectAsync(
            true,
            cancellationToken);
    }
}
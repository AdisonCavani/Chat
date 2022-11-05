using Chat.WebApi.Models.Settings;
using Chat.WebApi.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebApi.Services;

public class EmailService : IEmailService
{
    private readonly IOptionsSnapshot<SmtpSettings> _smtpSettings;

    public EmailService(IOptionsSnapshot<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }

    public async Task<bool> SendEmailAsync(
        string receiverName,
        string receiverEmail,
        string subject,
        string body,
        bool html = true,
        CancellationToken token = default)
    {
        try
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress(_smtpSettings.Value.Name, _smtpSettings.Value.Email));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            message.Body = new TextPart(html ? TextFormat.Html : TextFormat.Text)
            {
                Text = body
            };

            var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Value.Host, _smtpSettings.Value.Port, cancellationToken: token);
            await client.AuthenticateAsync(_smtpSettings.Value.Email, _smtpSettings.Value.Password, token);
            await client.SendAsync(message, token);
            await client.DisconnectAsync(true, token);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
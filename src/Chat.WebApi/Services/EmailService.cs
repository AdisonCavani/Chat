using Chat.WebApi.Models.Settings;
using Chat.WebApi.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
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
       bool html = true)
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

            // TODO: use cancellation token!
            var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Value.Host, _smtpSettings.Value.Port);
            await client.AuthenticateAsync(_smtpSettings.Value.Email, _smtpSettings.Value.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }
        catch
        {
            return false;
        }
    }
}

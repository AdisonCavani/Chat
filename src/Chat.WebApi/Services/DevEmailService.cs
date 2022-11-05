using Chat.WebApi.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebApi.Services;

public class DevEmailService : IEmailService
{
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
            message.From.Add(new MailboxAddress("Papercut", "chat@papercut.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            message.Body = new TextPart(html ? TextFormat.Html : TextFormat.Text)
            {
                Text = body
            };

            var client = new SmtpClient();
            await client.ConnectAsync("localhost", cancellationToken: token);
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
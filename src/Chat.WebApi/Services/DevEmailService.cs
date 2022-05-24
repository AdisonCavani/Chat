using Chat.WebApi.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Chat.WebApi.Services;

public class DevEmailService : IEmailService
{
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
            message.From.Add(new MailboxAddress("Papercut", "chat@papercut.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            message.Body = new TextPart(html ? TextFormat.Html : TextFormat.Text)
            {
                Text = body
            };

            // TODO: use cancellaction token!
            var client = new SmtpClient();
            await client.ConnectAsync("localhost");
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

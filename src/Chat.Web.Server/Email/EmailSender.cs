using System.Net.Mail;
using Chat.Core;

namespace Chat.Web.Server;

/// <summary>
/// Sends emails using the 
/// </summary>
public class EmailSender : IEmailSender
{
    public async Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details)
    {
        // Get the API key
        var apiKey = IoCContainer.Configuration["EmailKey"];

        // TODO: Create a new Email Client
        //var client = new SendGridClient(apiKey);

        // From
        var from = new MailAddress(details.FromEmail, details.FromName);

        // To
        var to = new MailAddress(details.ToEmail, details.ToName);

        // Subject
        var subject = details.Subject;

        // Content
        var content = details.Content;

        // TODO: Create Email class ready to send
        //var msg = MailHelper.CreateSingleEmail(from, to, subject, details.IsHTML ? null : details.Content : null);

        // Send email
        //var response = await client.SendEmailAsync(msg);

        // Return result based on response
        return new SendEmailResponse();
    }
}
using Chat.Core.DI.Interfaces;
using Chat.Core.Email;
using System.Reflection;
using System.Text;

namespace Chat.WebApi.Email.Templates;

public class EmailTemplateSender : IEmailTemplateSender
{
    private readonly IEmailSender _emailSender;

    public EmailTemplateSender(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
    {
        string templateText;
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var file = Path.Combine(dir, "Email", "Templates", "GeneralTemplate.html");

        // Read the general template from file
        // TODO: Replace with IoC Flat data provider
        using (var reader = new StreamReader(file, Encoding.UTF8))
        {
            // Read file contents
            templateText = await reader.ReadToEndAsync();
        }

        // Replace special values with those inside the template
        templateText = templateText.Replace("--Title--", title)
                                    .Replace("--Content1--", content1)
                                    .Replace("--Content2--", content2)
                                    .Replace("--ButtonText--", buttonText)
                                    .Replace("--ButtonUrl--", buttonUrl);

        // Set the details content to this template content
        details.Content = templateText;

        // Send email
        return await _emailSender.SendEmailAsync(details);
    }
}

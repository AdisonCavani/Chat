using System.Threading.Tasks;

namespace Chat.Core;

/// <summary>
/// A service that handles sending emails on behalf of the caller
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email message with the given information
    /// </summary>
    /// <returns></returns>
    Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details);
}

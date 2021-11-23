namespace Chat.Core;

/// <summary>
/// A response from a SendEmail call for any <see cref="IEmailSender"/> implementation
/// </summary>
public class SendEmailResponse
{
    /// <summary>
    /// True, if the email was sent successfully
    /// </summary>
    public bool Successful => ErrorMessage is null;

    /// <summary>
    /// The error message if the sending failed
    /// </summary>
    public string ErrorMessage { get; set; }
}
namespace Chat.WebApi.Email.SendGrid;

/// <summary>
/// A response to a SendGrid SendMessage call
/// </summary>
public class SendGridResponse
{
    /// <summary>
    /// Any errors from a response
    /// </summary>
    public List<SendGridResponseError> Errors { get; set; }
}

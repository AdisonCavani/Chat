using Chat.Core.DI.Interfaces;
using Chat.Core.Email;

namespace Chat.WebApi;

public static class FasettoEmailSender
{
    public static async Task<SendEmailResponse> SendUserVerificationEmailAsync(
        IConfiguration configuration,
        IEmailTemplateSender templateSender,
        string displayName,
        string email,
        string verificationUrl)
    {
        return await templateSender.SendGeneralEmailAsync(new SendEmailDetails
        {
            IsHTML = true,
            FromEmail = configuration["FasettoSettings:SendEmailFromEmail"],
            FromName = configuration["FasettoSettings:SendEmailFromName"],
            ToEmail = email,
            ToName = displayName,
            Subject = "Verify Your Email - Fasetto Word"
        },
        "Verify Email",
        $"Hi {displayName ?? "stranger"},",
        "Thanks for creating an account with us.<br/>To continue please verify your email with us.",
        "Verify Email",
        verificationUrl
        );
    }
}

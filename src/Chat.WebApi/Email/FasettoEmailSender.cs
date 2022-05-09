using Chat.Core.ApiModels.Email;
using Chat.Core.DI.Interfaces;

namespace Chat.WebApi.Email;

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

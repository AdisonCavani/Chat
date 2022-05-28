using Chat.Core;
using Chat.WebApi.Models.Entities;
using Chat.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Web;

namespace Chat.WebApi.Services;

// TODO: create HTML template handler
public class EmailHandler
{
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;

    public EmailHandler(IEmailService emailService, UserManager<AppUser> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<bool> SendVerificationEmailAsync(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        if (string.IsNullOrEmpty(token))
            return false;

        var name = $"{user.FirstName} {user.LastName}";
        var topic = "Confirm your email";

        var confirmationUrl = $"https://localhost:5001/{ApiRoutes.Account.ConfirmEmail}?userId={HttpUtility.UrlEncode(user.Id.ToString())}&token={HttpUtility.UrlEncode(token)}";

        var body = $"<a href='{confirmationUrl}'>Confirm email</a>";

        var emailSend = await _emailService.SendEmailAsync(name, user.Email, topic, body);

        return emailSend;
    }

    public async Task<bool> SendPasswordChangedAlertAsync(AppUser user)
    {
        var name = $"{user.FirstName} {user.LastName}";
        var topic = "Password has been changed";

        var body = $"<p>Your password has been changed</p>";

        var emailSend = await _emailService.SendEmailAsync(name, user.Email, topic, body);

        return emailSend;
    }

    public async Task<bool> SendPasswordRecoveryEmailAsync(AppUser user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (string.IsNullOrEmpty(token))
            return false;

        var name = $"{user.FirstName} {user.LastName}";
        var topic = "Password recovery";

        var body = $"<p>Token: {HttpUtility.UrlEncode(token)}</p>";

        var emailSend = await _emailService.SendEmailAsync(name, user.Email, topic, body);

        return emailSend;
    }
}

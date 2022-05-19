using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Chat.WebApi.Services;

public class EmailService
{
    private readonly UserManager<AppUser> _userManager;

    public EmailService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> SendVerificationEmail(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var smtpClient = new SmtpClient("localhost")
        {
            Port = 25,
        };

        MailMessage mailMessage = new()
        {
            From = new("chat@email.com"),
            Body = token
        };

        mailMessage.To.Add(user.Email);

        await smtpClient.SendMailAsync(mailMessage);

        return true;
    }
}

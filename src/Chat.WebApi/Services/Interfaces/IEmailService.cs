using System.Threading.Tasks;

namespace Chat.WebApi.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string receiverName, string receiverEmail, string subject, string body, bool html = true);
}

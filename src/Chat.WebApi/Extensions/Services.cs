using Chat.WebApi.Chat;
using Chat.WebApi.Services;
using Chat.WebApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.WebApi.Extensions;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services)
    {
#if DEBUG
        services.AddScoped<IEmailService, DevEmailService>();
#else
        services.AddScoped<IEmailService, EmailService>();
#endif
        services.AddScoped<EmailHandler>();
        services.AddScoped<JwtService>();

        services.AddTransient<ConnectionManager>();
        services.AddSingleton<ChatHandler>();
    }
}

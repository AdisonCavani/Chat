using Chat.WebApi.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.WebApi.Extensions;

public static class Settings
{
    public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
        services.Configure<AuthSettings>(configuration.GetSection(nameof(AuthSettings)));
        services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
    }
}

using Chat.Core;

namespace Chat.Web.Server;

/// <summary>
/// Extension methods for any Email classes
/// </summary>
public static class SendEmailExtensions
{
    /// <summary>
    /// Injects the <see cref="SendEmailSender"/> into the services to handle the <see cref="IEmailSender"/> service
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        // Inject the EmailSender
        services.AddTransient<IEmailSender, EmailSender>();

        // Return collection for chaining
        return services;
    }
}

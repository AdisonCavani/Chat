using Chat.WebApi.Models.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chat.WebApi.Extensions;

public static class DbContext
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration["DbSettings:SqlConnectionString"], configuration =>
            {
                configuration.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            });
        });
    }
}

using Chat.WebApi.Models.App;
using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using Chat.WebApi.Services;

namespace Chat.WebApi.Extensions;

public static class Identity
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = PasswordResetTokenProvider.ProviderKey;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider)
            .AddTokenProvider<PasswordResetTokenProvider>(PasswordResetTokenProvider.ProviderKey);

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15);

            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        });
    }
}
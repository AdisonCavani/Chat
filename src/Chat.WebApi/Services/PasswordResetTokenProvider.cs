using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Chat.WebApi.Services;

public class PasswordResetTokenProvider : TotpSecurityStampBasedTokenProvider<AppUser>
{
    public const string ProviderKey = "ResetPassword";

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
    {
        return Task.FromResult(false);
    }
}
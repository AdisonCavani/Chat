using Chat.WebApi.Models.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat.WebApi.Extensions;

public static class UserManagerExtensions
{
    public static Task<ApplicationUser?> FindByIdAsync(this UserManager<ApplicationUser> userManager, int userId)
    {
        return userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}

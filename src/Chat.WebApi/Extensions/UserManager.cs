using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Chat.WebApi.Extensions;

public static class UserManager
{
    public static Task<AppUser?> FindByIdAsync(this UserManager<AppUser> userManager, int userId)
    {
        return userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}

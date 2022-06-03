using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.WebApi.Models.App;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
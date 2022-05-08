using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.WebApi.Models.App;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public DbSet<SettingsDataModel> Settings { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent API
        modelBuilder.Entity<SettingsDataModel>().HasIndex(a => a.Name);
    }
}
using Microsoft.EntityFrameworkCore;

namespace Chat.WebApi.Data;

public class AppDbContext : DbContext
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

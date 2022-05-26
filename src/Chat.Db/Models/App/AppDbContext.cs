using Chat.Db.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Db.Models.App;

public class AppDbContext : DbContext
{
    public virtual DbSet<UserCredentials> UserCredentials { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCredentials>()
            .HasKey(c => c.Email);

        base.OnModelCreating(modelBuilder);
    }
}

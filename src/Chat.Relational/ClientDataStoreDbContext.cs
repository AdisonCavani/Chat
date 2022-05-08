using Chat.Core.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Chat.Relational;

/// <summary>
/// The database context for the client data store
/// </summary>
public class ClientDataStoreDbContext : DbContext
{
    /// <summary>
    /// The client login credentials
    /// </summary>
    public DbSet<LoginCredentialsDataModel> LoginCredentials { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public ClientDataStoreDbContext(DbContextOptions<ClientDataStoreDbContext> options) : base(options)
    {

    }

    /// <summary>
    /// Configures the database structure and relationships
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent API

        // Configure LoginCredentials
        // --------------------------
        //
        // Set Id as primary key
        modelBuilder.Entity<LoginCredentialsDataModel>().HasKey(a => a.Id);

        // TODO: Set up limits
        //modelBuilder.Entity<LoginCredentialsDataModel>().Property(a => a.FirstName).HasMaxLength(50);
    }
}

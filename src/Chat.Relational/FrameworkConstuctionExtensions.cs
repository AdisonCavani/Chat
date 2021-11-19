using Chat.Core;
using Dna;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Relational;

/// <summary>
/// Extension methods for the <see cref="FrameworkConstruction"/>
/// </summary>
public static class FrameworkConstuctionExtensions
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public static FrameworkConstruction UseClientDataStore(this FrameworkConstruction construction)
    {
        // Inject SQLite EF data store
        construction.Services.AddDbContext<ClientDataStoreDbContext>(options =>
        {
            // Setup connection string
            options.UseSqlite(construction.Configuration.GetConnectionString("ClientDataStoreConnection"));
        });

        // Add client data store for easy access/use of the backing data store
        // Make it scoped, so we can inject the scoped DbContext
        construction.Services.AddScoped<IClientDataStore>(provider => new ClientDataStore(provider.GetService<ClientDataStoreDbContext>()));

        // Return framework for chaining
        return construction;
    }
}

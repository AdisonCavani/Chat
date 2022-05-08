using Chat.Core.DataModels;
using Chat.Core.DI.Interfaces;

namespace Chat.Relational;

/// <summary>
/// Stores and retrieves information about the client application 
/// such as login credentials, messages, settings and so on
/// in an SQLite database
/// </summary>
public class BaseClientDataStore : IClientDataStore
{
    /// <summary>
    /// The database context for the client data store
    /// </summary>
    private readonly ClientDataStoreDbContext _context;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="context">The database to use</param>
    public BaseClientDataStore(ClientDataStoreDbContext context)
    {
        // Set local member
        _context = context;
    }

    /// <summary>
    /// Determines if the current user has logged in credentials
    /// </summary>
    public async Task<bool> HasCredentialsAsync()
    {
        return await GetLoginCredentialsAsync() is not null;
    }

    /// <summary>
    /// Makes sure the client data store is correctly set up
    /// </summary>
    /// <returns>Returns a task that will finish once setup is complete</returns>
    public async Task EnsureDataStoreAsync()
    {
        // Make sure the database exists and is created
        await _context.Database.EnsureCreatedAsync();
    }

    /// <summary>
    /// Gets the stored login credentials for this client
    /// </summary>
    /// <returns>Returns the login credentials if they exist, or null if none exist</returns>
    public Task<LoginCredentialsDataModel?> GetLoginCredentialsAsync()
    {
        // Get the first column in the login credentials table, or null if none exist
        return Task.FromResult(_context.LoginCredentials.FirstOrDefault());
    }

    /// <summary>
    /// Stores the given login credentials to the backing data store
    /// </summary>
    /// <param name="loginCredentials">The login credentials to save</param>
    /// <returns>Returns a task that will finish once the save is complete</returns>
    public async Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials)
    {
        // Clear all entries
        _context.LoginCredentials.RemoveRange(_context.LoginCredentials);

        // Add new one
        _context.LoginCredentials.Add(loginCredentials);

        // Save changes
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes all login credentials stored in the data store
    /// </summary>
    /// <returns></returns>
    public async Task ClearAllLoginCredentialsAsync()
    {
        // Clear all entries
        _context.LoginCredentials.RemoveRange(_context.LoginCredentials);

        // Save changes
        await _context.SaveChangesAsync();
    }
}

using System.Linq;
using System.Threading.Tasks;
using Chat.Core;

namespace Chat.Relational
{
    /// <summary>
    /// Stores and retrieves information about the client application
    /// such as login credentials, messages, settings and so on
    /// in an SQLite database
    /// </summary>
    public class BaseClientDataStore : IClientDataStore
    {
        #region Protected Members

        /// <summary>
        /// The database context for the client data store
        /// </summary>
        protected ClientDataStoreDbContext mDbContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContext">The database to use</param>
        public BaseClientDataStore(ClientDataStoreDbContext dbContext)
        {
            // Set local member
            mDbContext = dbContext;
        }

        #endregion

        #region Interface Implementation

        public async Task<bool> HasCredentialsAsync() => await GetLoginCredentialsAsync() is not null;

        public async Task EnsureDataStoreAsync()
        {
            // Make sure the database exist and is created
            await mDbContext.Database.EnsureCreatedAsync();
        }

        public Task<LoginCredentialsDataModel> GetLoginCredentialsAsync()
        {
            // Get the first column in the login credentials table, or null if none exist
            return Task.FromResult(mDbContext.LoginCredentials.FirstOrDefault());
        }

        public async Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials)
        {
            // Clear all entries
            // NOTE: might be slow, should rewrite to use TRUNCATE TABLE
            mDbContext.LoginCredentials.RemoveRange(mDbContext.LoginCredentials);

            // Add new one
            mDbContext.LoginCredentials.Add(loginCredentials);

            // Save changes
            await mDbContext.SaveChangesAsync();
        }

        #endregion
    }
}

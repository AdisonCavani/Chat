using Chat.Db.Models.App;
using Chat.Db.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Chat.Services;

public class UserCredentialsManager
{
    private readonly AppDbContext _context;

    public UserCredentialsManager(AppDbContext context)
    {
        _context = context;
    }

    /// <returns>True, if database was created</returns>
    public async Task<bool> EnsureCreatedAsync()
    {
        return await _context.Database.EnsureCreatedAsync();
    }

    public async Task<bool> SaveUserCredentials(UserCredentials credentials)
    {
        _context.UserCredentials.RemoveRange(_context.UserCredentials);
        await _context.UserCredentials.AddAsync(credentials);
        var saved = await _context.SaveChangesAsync();

        return saved > 0;
    }

    public async Task<UserCredentials> GetUserCredentialsAsync()
    {
        return await _context.UserCredentials.FirstOrDefaultAsync();
    }

    public async Task<bool> ClearUserCredentialsAsync()
    {
        _context.UserCredentials.RemoveRange(_context.UserCredentials);
        var saved = await _context.SaveChangesAsync();

        return saved > 0;
    }
}

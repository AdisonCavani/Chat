using Microsoft.AspNetCore.Identity;

namespace Chat.WebApi.Data;

/// <summary>
/// The user data and profile for our application
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// The users first name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The users last name
    /// </summary>
    public string LastName { get; set; }
}


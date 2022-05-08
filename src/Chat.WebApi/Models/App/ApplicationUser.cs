using Microsoft.AspNetCore.Identity;

namespace Chat.WebApi.Models.App;

/// <summary>
/// The user data and profile for our application
/// </summary>
public class ApplicationUser : IdentityUser<int>
{
    /// <summary>
    /// The users first name
    /// </summary>
    [PersonalData]
    public string FirstName { get; set; }

    /// <summary>
    /// The users last name
    /// </summary>
    [PersonalData]
    public string LastName { get; set; }
}


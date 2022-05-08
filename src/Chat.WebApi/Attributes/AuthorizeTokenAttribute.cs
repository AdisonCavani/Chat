using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Chat.WebApi.Attributes;

/// <summary>
/// The authorization policy for token-based authentication
/// </summary>
public class AuthorizeTokenAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public AuthorizeTokenAttribute()
    {
        // Add the JWT bearer authentication scheme
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}
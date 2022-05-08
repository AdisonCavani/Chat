using Chat.WebApi.Models.App;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.WebApi.Extensions;

public static class JwtTokenExtensionMethods
{
    public static string GenerateJwtToken(this ApplicationUser user, IConfiguration configuration)
    {
        var claims = new[]
        {
            // Unique ID for this token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

            // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),

            // Add user Id so that UserManager.GetUserAsync can find the user based on Id
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            SecurityAlgorithms.HmacSha256);

        // Generate the Jwt Token
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.Now.AddMonths(3));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

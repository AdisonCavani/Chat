using Chat.WebApi.Models.App;
using Chat.WebApi.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.WebApi.Services;

public class TokenService
{
    private readonly IOptionsSnapshot<AuthSettings> _configuration;

    public TokenService(IOptionsSnapshot<AuthSettings> configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(ApplicationUser user)
    {
        List<Claim> claims = new()
        {
            // Unique ID for this token
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

            // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
            new(ClaimsIdentity.DefaultNameClaimType, user.UserName),

            // Add user Id so that UserManager.GetUserAsync can find the user based on Id
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        // Generate the Jwt Token
        var token = new JwtSecurityToken(
            issuer: _configuration.Value.Issuer,
            audience: _configuration.Value.Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.Now.AddMonths(3));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

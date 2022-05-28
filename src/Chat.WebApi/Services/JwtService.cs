using Chat.WebApi.Models.App;
using Chat.WebApi.Models.Entities;
using Chat.WebApi.Models.Internal;
using Chat.WebApi.Models.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.WebApi.Services;

public class JwtService
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOptionsSnapshot<AuthSettings> _authSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtService(
        AppDbContext context,
        UserManager<AppUser> userManager,
        IOptionsSnapshot<AuthSettings> authSettings,
        TokenValidationParameters tokenValidationParameters)
    {
        _context = context;
        _userManager = userManager;
        _authSettings = authSettings;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<AuthenticationResult> GenerateTokenAsync(AppUser user)
    {
        return await GenerateAuthenticationResultForUser(user);
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);

        if (validatedToken == null)
            return new()
            {
                Errors = new[] { "Invalid token" }
            };

        var expirationDataUnix =
            long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expirationDateTimeUtc = DateTime.UnixEpoch.AddSeconds(expirationDataUnix);

        if (expirationDateTimeUtc > DateTime.UtcNow)
            return new()
            {
                Errors = new[] { "This token hasn't expired yet" }
            };

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

        if (storedRefreshToken is null)
            return new()
            {
                Errors = new[] { "This token doesn't exist" }
            };

        if (DateTime.UtcNow > storedRefreshToken.ExpirationDate)
            return new()
            {
                Errors = new[] { "This refresh token has expired" }
            };

        if (storedRefreshToken.Invalidated)
            return new()
            {
                Errors = new[] { "This refresh token has been invalidated" }
            };

        // TODO: invalidate all tokens for security
        // SEE: https://auth0.com/blog/refresh-tokens-what-are-they-and-when-to-use-them/#Refresh-Token-Automatic-Reuse-Detection
        if (storedRefreshToken.Used)
        {
            var usedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.JwtId == storedRefreshToken.JwtId);

            if (usedToken is null)
                return new()
                {
                    Errors = new[] { "This refresh token has been used" }
                };

            await _context.RefreshTokens
                .Where(x => x.UserId == usedToken.UserId)
                .ForEachAsync(c => c.Invalidated = true);
            await _context.SaveChangesAsync();

            return new()
            {
                Errors = new[] { "This refresh token has been used" }
            };
        }

        if (storedRefreshToken.JwtId != jti)
            return new()
            {
                Errors = new[] { "This refresh token does not match this JWT" }
            };

        storedRefreshToken.Used = true;
        _context.RefreshTokens.Update(storedRefreshToken);
        await _context.SaveChangesAsync(); // TODO: validate save

        var user = await _userManager.FindByIdAsync(validatedToken.Claims
            .Single(x => x.Type == JwtRegisteredClaimNames.NameId).Value);

        return await GenerateAuthenticationResultForUser(user);
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                return null;

            return principal;
        }
        catch
        {
            return null; // TODO: add logging?
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken? validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultForUser(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey));

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new(new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
            }),
            Expires = DateTime.UtcNow.AddMinutes(_authSettings.Value.ExpireMinutes),
            Audience = _authSettings.Value.Audience,
            Issuer = _authSettings.Value.Issuer,
            SigningCredentials = new(key, SecurityAlgorithms.HmacSha256Signature) // TODO: verify algorithm
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        RefreshToken refreshToken = new()
        {
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(_authSettings.Value.ExpireDays)
        };

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync(); // TODO: validate save

        return new()
        {
            Success = true,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken.Token
        };
    }
}
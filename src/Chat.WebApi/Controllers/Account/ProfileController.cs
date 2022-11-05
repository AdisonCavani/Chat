using Chat.Core;
using Chat.Core.Models.Entities;
using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Threading.Tasks;
using ControllerBase = Chat.WebApi.Extensions.ControllerBase;

namespace Chat.WebApi.Controllers.Account;

public class ProfileController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;

    public ProfileController(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [Authorize]
    [HttpGet(ApiRoutes.Account.Profile.Details)]
    public async Task<ActionResult<UserProfile>> GetUserDetailsAsync()
    {
        var uid = HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(uid))
            return InternalServerError();

        var user = await _signInManager.UserManager.FindByIdAsync(uid);

        if (user is null)
            return InternalServerError();

        return Ok(new UserProfile
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        });
    }
}
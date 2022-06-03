using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.WebApi.Models.Entities;
using Chat.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ControllerBase = Chat.WebApi.Extensions.ControllerBase;

namespace Chat.WebApi.Controllers.Account;

[ApiController]
public class PasswordController : ControllerBase
{
    private readonly EmailHandler _emailHandler;
    private readonly UserManager<AppUser> _userManager;

    public PasswordController(EmailHandler emailHandler, UserManager<AppUser> userManager)
    {
        _emailHandler = emailHandler;
        _userManager = userManager;
    }

    [HttpGet(ApiRoutes.Account.Password.SendRecoveryEmail)]
    public async Task<IActionResult> SendRecoveryEmailAsync([FromQuery] PasswordRecoveryDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "Couldn't find user associated with this email" }
            });

        var emailHandled = await _emailHandler.SendPasswordRecoveryEmailAsync(user);

        return emailHandled
            ? Ok()
            : InternalServerError();
    }

    [HttpPost(ApiRoutes.Account.Password.VerifyToken)]
    public async Task<IActionResult> VerifyTokenAsync([FromBody] PasswordRecoveryTokenDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "Couldn't find user associated with this email" }
            });

        if (!await _userManager.VerifyUserTokenAsync(user, PasswordResetTokenProvider.ProviderKey,
                PasswordResetTokenProvider.ProviderKey, dto.Token))
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "Invalid token" }
            });

        return Ok();
    }

    [HttpPost(ApiRoutes.Account.Password.Reset)]
    public async Task<IActionResult> ResetAsync([FromBody] ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return InternalServerError();

        var result =
            await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

        if (!result.Succeeded)
            return BadRequest(new ApiResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });

        var emailHandled = await _emailHandler.SendPasswordChangedAlertAsync(user);

        return emailHandled
            ? Ok()
            : InternalServerError();
    }

    [Authorize]
    [HttpPost(ApiRoutes.Account.Password.Change)]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var uid = HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(uid))
            return InternalServerError();

        var user = await _userManager.FindByIdAsync(uid);

        if (user is null)
            return InternalServerError();

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword,
            dto.NewPassword); // TODO: password might be the same!

        if (!result.Succeeded)
            return BadRequest(new ApiResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });

        var emailHandled = await _emailHandler.SendPasswordChangedAlertAsync(user);

        return emailHandled
            ? Ok()
            : InternalServerError();
    }
}
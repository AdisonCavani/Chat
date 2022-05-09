using Chat.Core.ApiModels;
using Chat.Core.ApiModels.LoginRegister;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.Routes;
using Chat.WebApi.Email;
using Chat.WebApi.Extensions;
using Chat.WebApi.Models.App;
using Chat.WebApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Chat.WebApi.Controllers;

[ApiController]
public class AccountConroller : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;

    public AccountConroller(UserManager<ApplicationUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost(ApiRoutes.Register)]
    public async Task<ApiResponse<RegisterResultDto>> RegisterAsync([FromBody] RegisterCredentialsDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return new ApiResponse<RegisterResultDto>
            {
                ErrorMessage = result.Errors.AggregateErrors()
            };

        var userIdentity = await _userManager.FindByNameAsync(user.UserName);
        // await SendUserEmailVerificationAsync(user);

        return new ApiResponse<RegisterResultDto>
        {
            Response = new RegisterResultDto
            {
                FirstName = userIdentity.FirstName,
                LastName = userIdentity.LastName,
                Email = userIdentity.Email,
                Username = userIdentity.UserName,
                Token = _tokenService.GenerateJwtToken(userIdentity)
            }
        };
    }

    [HttpPost(ApiRoutes.Login)]
    public async Task<ApiResponse<UserProfileDetailsDto>> LogInAsync([FromBody] LoginCredentialsDto dto)
    {
        var errorResponse = new ApiResponse<UserProfileDetailsDto>
        {
            // TODO: Localize all strings
            ErrorMessage = "Invalid username or password"
        };

        var isEmail = dto.UsernameOrEmail.Contains('@');
        var user = isEmail
                    ? await _userManager.FindByEmailAsync(dto.UsernameOrEmail)
                    : await _userManager.FindByNameAsync(dto.UsernameOrEmail);

        if (user is null)
            return errorResponse;

        var isValidPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isValidPassword)
            return errorResponse;

        return new ApiResponse<UserProfileDetailsDto>
        {
            Response = new UserProfileDetailsDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                Token = _tokenService.GenerateJwtToken(user)
            }
        };
    }

    [HttpGet(ApiRoutes.VerifyEmail)]
    public async Task<IActionResult> VerifyEmailAsync([FromQuery] VerifyUserEmailDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null)
            return BadRequest();

        var result = await _userManager.ConfirmEmailAsync(user, dto.EmailToken);

        if (result.Succeeded)
            return Ok();

        return BadRequest();
    }

    private async Task SendUserEmailVerificationAsync(ApplicationUser user)
    {
        var userIdentity = await _userManager.FindByNameAsync(user.UserName);
        var emailVerificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // TODO: Replace with APIRoutes that will contain the static routes to use
        var confirmationUrl =
            $"http://{Request.Host.Value}/api/verify/email/?userId={HttpUtility.UrlEncode(BitConverter.GetBytes(userIdentity.Id))}&emailToken={HttpUtility.UrlEncode(emailVerificationCode)}";

        // TODO: add email validation (if was send)
        await FasettoEmailSender.SendUserVerificationEmailAsync(null, null, user.UserName, userIdentity.Email, confirmationUrl);
    }
}

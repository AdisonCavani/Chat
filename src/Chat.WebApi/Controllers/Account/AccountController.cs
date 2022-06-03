using Chat.Core;
using Chat.Core.Models.Entities;
using Chat.Core.Models.Requests;
using Chat.Core.Models.Responses;
using Chat.WebApi.Extensions;
using Chat.WebApi.Models.Entities;
using Chat.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ControllerBase = Chat.WebApi.Extensions.ControllerBase;

namespace Chat.WebApi.Controllers.Account;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly EmailHandler _emailHandler;
    private readonly JwtService _jwtService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(EmailHandler emailHandler, JwtService jwtService, SignInManager<AppUser> signInManager)
    {
        _emailHandler = emailHandler;
        _jwtService = jwtService;
        _signInManager = signInManager;
    }

    [HttpPost(ApiRoutes.Account.Register)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterCredentialsDto dto)
    {
        AppUser user = new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email // TODO: Resolve this duplication
        };

        var result = await _signInManager.UserManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(new ApiResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });

        var createdUser = await _signInManager.UserManager.FindByEmailAsync(dto.Email);

        if (createdUser is null)
            return InternalServerError();

        var emailHandled = await _emailHandler.SendVerificationEmailAsync(user);

        return emailHandled
            ? Ok()
            : InternalServerError();
    }

    [HttpGet(ApiRoutes.Account.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailDto dto)
    {
        var user = await _signInManager.UserManager.FindByIdAsync(dto.UserId);

        if (user is null)
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "Couldn't find user associated with this id" }
            });

        var emailConfirmed = await _signInManager.UserManager.IsEmailConfirmedAsync(user);

        if (emailConfirmed)
            return Conflict(new ApiResponse
            {
                Errors = new[] { "Email is already confirmed" }
            });

        var result = await _signInManager.UserManager.ConfirmEmailAsync(user, dto.Token);

        return result.Succeeded
            ? Ok()
            : BadRequest(new ApiResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });
    }

    [HttpPost(ApiRoutes.Account.Login)]
    public async Task<ActionResult<JwtTokenDto>> LoginAsync([FromBody] LoginCredentialsDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);

        if (result.IsNotAllowed)
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "Confirm your email" }
            });

        if (result.IsLockedOut)
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "User is locked out" }
            });

        if (!result.Succeeded)
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "Wrong credentials" }
            });

        var user = await _signInManager.UserManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return InternalServerError();

        var token = _jwtService.GenerateToken(user);

        return Ok(new ApiResponse<JwtTokenDto>
        {
            Result = new()
            {
                Token = token.Token,
            }
        });
    }

    [HttpGet(ApiRoutes.Account.ResendVerificationEmail)]
    public async Task<IActionResult> ResendVerificationEmailAsync([FromQuery] ResendVerificationEmailDto dto)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return BadRequest(new ApiResponse
            {
                Errors = new[] { "Couldn't find user associated with this email" }
            });

        var emailConfirmed = await _signInManager.UserManager.IsEmailConfirmedAsync(user);

        if (emailConfirmed)
            return Conflict(new ApiResponse
            {
                Errors = new[] { "Email is already confirmed" }
            });

        var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

        if (token is null)
            return InternalServerError();

        var result = await _emailHandler.SendVerificationEmailAsync(user);

        return result
            ? Ok()
            : InternalServerError();
    }

    [Authorize]
    [HttpGet(ApiRoutes.Account.Auth)]
    public async Task<ActionResult<UserProfile>> GetUserDetails()
    {
        var uid = HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(uid))
            return InternalServerError();

        var user = await _signInManager.UserManager.FindByIdAsync(uid);

        if (user is null)
            return InternalServerError();

        return Ok(new ApiResponse<UserProfile>
        {
            Result = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            }
        });
    }
}
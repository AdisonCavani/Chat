using Chat.Core.Models.Requests;
using Chat.Core.Models.Responses;
﻿using Chat.Core;
using Chat.WebApi.Extensions;
using Chat.WebApi.Models.Entities;
using Chat.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chat.WebApi.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly JwtService _jwtService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(EmailService emailService, JwtService jwtService, SignInManager<AppUser> signInManager)
    {
        _emailService = emailService;
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
            return BadRequest(result.Errors);

        var createdUser = await _signInManager.UserManager.FindByEmailAsync(dto.Email);
        await _emailService.SendVerificationEmail(createdUser);

        return Ok();
    }

    [HttpPost(ApiRoutes.Account.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailDto dto)
    {
        var user = await _signInManager.UserManager.FindByIdAsync(dto.UserId);

        if (user is null)
            return BadRequest();

        var emailConfirmed = await _signInManager.UserManager.IsEmailConfirmedAsync(user);

        if (emailConfirmed)
            return Conflict("Email is already confirmed");

        var result = await _signInManager.UserManager.ConfirmEmailAsync(user, dto.Token);

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    [HttpPost(ApiRoutes.Account.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCredentialsDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);

        if (result.IsNotAllowed)
            return BadRequest("Confirm your email");

        if (result.IsLockedOut)
            return Ok("User is locked out");

        if (!result.Succeeded)
            return BadRequest();

        var user = await _signInManager.UserManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return new StatusCodeResult(500);

        var token = await _jwtService.GenerateTokenAsync(user);

        return Ok(new RefreshTokenDto
        {
            Token = token.Token,
            RefreshToken = token.RefreshToken,
        });
    }

    [HttpPost(ApiRoutes.Account.RefreshToken)]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto dto)
    {
        var response = await _jwtService.RefreshTokenAsync(dto.Token, dto.RefreshToken);

        if (!response.Success)
            return BadRequest(response.Errors);

        return Ok(new AuthSuccessResponse
        {
            Token = response.Token,
            RefreshToken = response.RefreshToken,
        });
    }

    [Authorize]
    [HttpGet(ApiRoutes.Account.Auth)]
    public IActionResult Test()
    {
        return Ok();
    }
}

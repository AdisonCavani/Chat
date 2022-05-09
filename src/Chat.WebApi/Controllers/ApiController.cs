using Chat.Core.ApiModels;
using Chat.Core.ApiModels.Contacts;
using Chat.Core.ApiModels.LoginRegister;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.DI.Interfaces;
using Chat.Core.Routes;
using Chat.WebApi.Email;
using Chat.WebApi.Extensions;
using Chat.WebApi.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Chat.WebApi.Controllers;

[Authorize]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IEmailTemplateSender _templateSender;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApiController(
        IConfiguration configuration,
        IEmailTemplateSender templateSender,
        UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _templateSender = templateSender;
        _userManager = userManager;
    }

    [HttpGet(ApiRoutes.GetUserProfile)]
    public async Task<ApiResponse<UserProfileDetailsDto>> GetUserProfileAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user is null)
            return new ApiResponse<UserProfileDetailsDto>()
            {
                // TODO: Localization
                ErrorMessage = "User not found"
            };

        return new ApiResponse<UserProfileDetailsDto>
        {
            Response = new UserProfileDetailsDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName
            }
        };
    }

    [HttpPatch(ApiRoutes.UpdateUserProfile)]
    public async Task<ApiResponse> UpdateUserProfileAsync([FromBody] UpdateUserProfileDto dto)
    {
        var emailChanged = false;
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user is null)
            return new ApiResponse
            {
                // TODO: Localization
                ErrorMessage = "User not found"
            };

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;

        if (!string.Equals(dto.Email, user.NormalizedEmail, StringComparison.OrdinalIgnoreCase))
        {
            user.Email = dto.Email;
            user.EmailConfirmed = false;
            emailChanged = true;
        }
        user.UserName = dto.Username;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded && emailChanged)
            await SendUserEmailVerificationAsync(user);

        if (result.Succeeded)
            return new ApiResponse();

        return new ApiResponse
        {
            ErrorMessage = result.Errors.AggregateErrors()
        };
    }

    [HttpPatch(ApiRoutes.UpdateUserPassword)]
    public async Task<ApiResponse> UpdateUserPasswordAsync([FromBody] UpdateUserPasswordDto model)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user is null)
            return new ApiResponse
            {
                // TODO: Localization
                ErrorMessage = "User not found"
            };

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
            return new ApiResponse();

        return new ApiResponse
        {
            ErrorMessage = result.Errors.AggregateErrors()
        };
    }

    [HttpPost(ApiRoutes.SearchUsers)]
    public async Task<ApiResponse<SearchUsersResultsDto>> SearchUsersAsync([FromBody] SearchUsersDto model)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user is null)
            return new ApiResponse<SearchUsersResultsDto>
            {
                // TODO: Localization
                ErrorMessage = "User not found"
            };

        var foundUser = await _userManager.FindByNameAsync(model.Username);

        if (foundUser is null)
            foundUser = await _userManager.FindByEmailAsync(model.Email);

        if (foundUser is null)
            foundUser = _userManager.Users.FirstOrDefault(u =>
                u.PhoneNumberConfirmed &&
                u.PhoneNumber == model.PhoneNumber);

        if (foundUser is not null)
            return new ApiResponse<SearchUsersResultsDto>
            {
                Response = new SearchUsersResultsDto
                {
                    new()
                    {
                        Username = foundUser.UserName,
                        FirstName = foundUser.FirstName,
                        LastName = foundUser.LastName
                    }
                }
            };

        SearchUsersResultsDto results = new();

        // TODO: fix this validation!!!
        // If we have a first and last name...
        // Search for users...
        var foundUsers = _userManager.Users.Where(u =>
                u.FirstName == model.FirstName &&
                u.LastName == model.LastName)
            // And for now, limit to 100 results
            // TODO: Add pagination
            .Take(100);

        if (foundUsers.Any())
            results.AddRange(foundUsers.Select(u => new SearchUsersResultDto
            {
                Username = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName
            }));

        return new ApiResponse<SearchUsersResultsDto>
        {
            Response = results
        };
    }

    private async Task SendUserEmailVerificationAsync(ApplicationUser user)
    {
        var userIdentity = await _userManager.FindByNameAsync(user.UserName);
        var emailVerificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // TODO: Replace with APIRoutes that will contain the static routes to use
        var confirmationUrl =
            $"http://{Request.Host.Value}/api/verify/email/?userId={HttpUtility.UrlEncode(BitConverter.GetBytes(userIdentity.Id))}&emailToken={HttpUtility.UrlEncode(emailVerificationCode)}";

        // TODO: add email validation (if was send)
        await FasettoEmailSender.SendUserVerificationEmailAsync(_configuration, _templateSender, user.UserName,
            userIdentity.Email, confirmationUrl);
    }
}
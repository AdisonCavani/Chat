using System.ComponentModel.DataAnnotations;
using Chat.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web.Server.Controllers;

/// <summary>
/// Manages the Web API calls
/// </summary>
public class ApiController : Controller
{
    #region Protected Members

    /// <summary>
    /// The scoped Application context
    /// </summary>
    protected ApplicationDbContext mContext;

    /// <summary>
    /// The manager for handling user creation, deletion, searching, roles etc...
    /// </summary>
    protected UserManager<ApplicationUser> mUserManager;

    /// <summary>
    /// The manager for handling signing in and out for our users
    /// </summary>
    protected SignInManager<ApplicationUser> mSignInManager;

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="context">The injected context</param>
    /// <param name="userManager">The identity user manager</param>
    /// <param name="signInManager">The identity sign in manager</param>
    public ApiController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
    {
        mContext = context;
        mUserManager = userManager;
        mSignInManager = signInManager;
    }

    #endregion

    /// <summary>
    /// Tries to register for a new account on the server
    /// </summary>
    /// <param name="registerCredentials">The registration details</param>
    /// <returns>Returns the result of the register request</returns>
    [Route("api/register")]
    public async Task<ApiResponse<RegisterResultApiModel>> RegisterAsync([FromBody] RegisterCredentialsApiModel registerCredentials)
    {
        // TODO: Localize all strings
        // The message when we fail to login
        var invalidErrorMessage = "Please provide all required details to register for an account";

        // The error response for a failed login
        var errorResponse = new ApiResponse<RegisterResultApiModel>
        {
            // TODO: Localize all strings
            // Set error message
            ErrorMessage = invalidErrorMessage
        };

        // Check, if credentials exist
        if (registerCredentials is null)
            // Return failed response
            return errorResponse;

        // Make sure we have a email
        if (string.IsNullOrWhiteSpace(registerCredentials.Email))
            // Return error message to user
            return errorResponse;

        // Create the desired user from the given details
        var user = new ApplicationUser
        {
            FirstName = registerCredentials.FirstName,
            LastName = registerCredentials.LastName,
            UserName = registerCredentials.Email,
            Email = registerCredentials.Email
        };

        // Try and create a user
        var result = await mUserManager.CreateAsync(user, registerCredentials.Password);

        if (result.Succeeded)
        {
            // Get the user details
            var userIdentity = await mUserManager.FindByEmailAsync(registerCredentials.Email);

            // Generate an email verification code
            var emailVerificationCode = mUserManager.GenerateEmailConfirmationTokenAsync(user);

            // TODO: Email the user the verification code

            // Return valid response containing all users details
            return new ApiResponse<RegisterResultApiModel>
            {
                Response = new RegisterResultApiModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.Email,
                    Token = user.GenerateJwtToken()
                }
            };
        }

        else
            // Return failed response
            return new ApiResponse<RegisterResultApiModel>
            {
                // Aggregate all errors into a single error string
                ErrorMessage = result.Errors.ToList()
                .Select(f => f.Description)
                .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}")
            };
    }

    /// <summary>
    /// Logs in a user using token-based authentication
    /// </summary>
    /// <returns></returns>
    [Route("api/login")]
    public async Task<ApiResponse<LoginResultApiModel>> LogInAsync([FromBody] LoginCredentialsApiModel loginCredentials)
    {
        // TODO: Localize all strings
        // The message when we fail to login
        var invalidErrorMessage = "Invalid email or password";

        // The error response for a failed login
        var errorResponse = new ApiResponse<LoginResultApiModel>
        {
            // TODO: Localize all strings
            // Set error message
            ErrorMessage = invalidErrorMessage
        };

        // Make sure we have a email
        if (string.IsNullOrWhiteSpace(loginCredentials.Email))
            // Return error message to user
            return errorResponse;

        // Validate if the user credentials are correct

        // TODO: Improved method
        // Is this an valid email?
        var isEmailValid = new EmailAddressAttribute().IsValid(loginCredentials.Email);

        if (!isEmailValid)
            return errorResponse;

        var user = await mUserManager.FindByEmailAsync(loginCredentials.Email);


        // If we failed to find a user
        if (user is null)
            // Return error message to user
            return errorResponse;

        // We have a user, validate a password
        var isValidPassword = await mUserManager.CheckPasswordAsync(user, loginCredentials.Password);

        // Password was wrong
        if (!isValidPassword)
            // Return error message to user
            return errorResponse;

        // Validated email and password

        // Return token to user
        return new ApiResponse<LoginResultApiModel>
        {
            // Pass back the user details and the token
            Response = new LoginResultApiModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Email,
                Token = user.GenerateJwtToken()
            }
        };
    }

    /// <summary>
    /// Test private area for token-based authentication
    /// </summary>
    /// <returns></returns>
    [AuthorizeToken]
    [Route("api/private")]
    public IActionResult Private()
    {
        // Get the authenticated user
        var user = HttpContext.User;

        // Tell them a secret
        return Ok(new { privateData = $"some secret for {user.Identity.Name}" });
    }
}

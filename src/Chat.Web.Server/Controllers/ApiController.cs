﻿using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Web.Server.Controllers
{
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
        /// Logs in a user using token-based authentication
        /// </summary>
        /// <returns></returns>
        [Route("api/login")]
        public async Task<ApiResponse<LoginResultApiModel>> LogInAsync([FromBody] LoginCredentialsApiModel loginCredentials)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Invalid username/email or password";

            // The error response for a failed login
            var errorResponse = new ApiResponse<LoginResultApiModel>
            {
                // TODO: Localize all strings
                // Set error message
                ErrorMessage = invalidErrorMessage
            };

            // Make sure  we have a username
            if (loginCredentials?.UsernameOrEmail is null || string.IsNullOrWhiteSpace(loginCredentials.UsernameOrEmail))
                // Return error message to user
                return errorResponse;

            // Validate if the user credentials are correct

            // TODO: Improved method
            // Is this an email?
            var isEmail = new EmailAddressAttribute().IsValid(loginCredentials.UsernameOrEmail);

            // Get the user details
            var user = isEmail ?
                // Find by email
                await mUserManager.FindByEmailAsync(loginCredentials.UsernameOrEmail) :
                // Find by username
                await mUserManager.FindByNameAsync(loginCredentials.UsernameOrEmail);

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

            // Validated username and password

            // Get username
            var username = user.UserName;

            // Set our tokens claims
            var claims = new[]
            {
                // Unique ID for this token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
            };

            // Create the credentials used to generate the token
            var credentials = new SigningCredentials(
                // Get the secret key from configuration
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IoCContainer.Configuration["Jwt:SecretKey"])),
                // Use HS256 algorithm
                SecurityAlgorithms.HmacSha256);

            // Generate the Jwt Token
            var token = new JwtSecurityToken(
                issuer: IoCContainer.Configuration["Jwt:Issuer"],
                audience: IoCContainer.Configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials,
                // Expire if not used for 3 months
                expires: DateTime.Now.AddMonths(3)
                );

            // Return token to user
            return new ApiResponse<LoginResultApiModel>
            {
                // Pass back the user details and the token
                Response = new LoginResultApiModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
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
}

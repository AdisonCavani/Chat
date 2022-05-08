﻿using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Core.ApiModels;
using Chat.Core.ApiModels.LoginRegister;
using Chat.Core.DataModels;
using Chat.Core.Routes;
using Chat.Core.Security;
using Chat.ViewModel.Base;
using Chat.WebRequests;
using static Chat.DI.DI;

namespace Chat.ViewModel.Application;

/// <summary>
/// The View Model for a register screen
/// </summary>
public class RegisterViewModel : BaseViewModel
{
    #region Public Properties

    /// <summary>
    /// The username of the user
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The email of the user
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// A flag indicating if the register command is running
    /// </summary>
    public bool RegisterIsRunning { get; set; }

    #endregion

    #region Commands

    /// <summary>
    /// The command to login
    /// </summary>
    public ICommand LoginCommand { get; set; }

    /// <summary>
    /// The command to register for a new account
    /// </summary>
    public ICommand RegisterCommand { get; set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public RegisterViewModel()
    {
        // Create commands
        RegisterCommand = new RelayParameterizedCommand(async (parameter) => await RegisterAsync(parameter));
        LoginCommand = new RelayCommand(async () => await LoginAsync());
    }

    #endregion

    /// <summary>
    /// Attempts to register a new user
    /// </summary>
    /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
    /// <returns></returns>
    public async Task RegisterAsync(object parameter)
    {
        await RunCommandAsync(() => RegisterIsRunning, async () =>
        {
            // Call the server and attempt to register with the provided credentials
            var result = await Dna.WebRequests.PostAsync<ApiResponse<RegisterResultDto>>(
            // Set URL
                RouteHelpers.GetAbsoluteRoute(ApiRoutes.Register),
                // Create api model
                new RegisterCredentialsDto
                {
                    Username = Username,
                    Email = Email,
                    Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                });

            // If the response has an error...
            if (await result.HandleErrorIfFailedAsync("Register Failed"))
                // We are done
                return;

            // OK successfully registered (and logged in)... now get users data
            var loginResult = result.ServerResponse.Response;

            // Let the application view model handle what happens
            // with the successful login
            await ApplicationViewModel.HandleSuccessfulLoginAsync(loginResult);
        });
    }

    /// <summary>
    /// Takes the user to the login page
    /// </summary>
    /// <returns></returns>
    public async Task LoginAsync()
    {
        ViewModelApplication.GoToPage(ApplicationPage.Login);
    }
}

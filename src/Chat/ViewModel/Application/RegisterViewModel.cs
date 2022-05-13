using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Core.ApiModels;
using Chat.Core.ApiModels.LoginRegister;
using Chat.Core.DataModels;
using Chat.Core.Extensions;
using Chat.Core.Routes;
using Chat.ViewModel.Base;
using Chat.WebRequests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Chat.DI.DI;

namespace Chat.ViewModel.Application;

/// <summary>
/// The View Model for a register screen
/// </summary>
public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    private string? username;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private bool registerIsRunning;

    [ICommand]
    public async Task Register(object parameter)
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
                    // TODO: add first and last name
                    FirstName = "test",
                    LastName = "test",
                    Username = Username ?? string.Empty,
                    Email = Email ?? string.Empty,
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
            if (loginResult is not null)
                await ApplicationViewModel.HandleSuccessfulLoginAsync(loginResult);
        });
    }

    [ICommand]
    public static void Login()
    {
        ViewModelApplication.GoToPage(ApplicationPage.Login);
    }

    // TODO: remove legacy BaseViewModel helpers
    private readonly object mPropertyValueCheckLock = new();

    private async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
    {
        lock (mPropertyValueCheckLock)
        {
            if (updatingFlag.GetPropertyValue())
                return;

            updatingFlag.SetPropertyValue(true);
        }

        try
        {
            await action();
        }
        finally
        {
            updatingFlag.SetPropertyValue(false);
        }
    }
}

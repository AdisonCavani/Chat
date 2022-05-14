using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Core.ApiModels;
using Chat.Core.ApiModels.LoginRegister;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.DataModels;
using Chat.Core.Extensions;
using Chat.Core.Routes;
using Chat.DI.UI;
using Chat.ViewModels.Base;
using Chat.WebRequests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chat.ViewModels.Application;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApplicationViewModel _applicationViewModel;
    private readonly SettingsViewModel _settingsViewModel;
    private readonly IUIManager _uiManager;

    public LoginViewModel(ApplicationViewModel applicationViewModel, SettingsViewModel settingsViewModel, IUIManager uiManager)
    {
        _applicationViewModel = applicationViewModel;
        _settingsViewModel = settingsViewModel;
        _uiManager = uiManager;
    }

    public LoginViewModel()
    {

    }

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private bool loginIsRunning;

    [ICommand]
    private async Task Login(object parameter)
    {
        await RunCommandAsync(() => LoginIsRunning, async () =>
        {
            var result = await Dna.WebRequests.PostAsync<ApiResponse<UserProfileDetailsDto>>(
                RouteHelpers.GetAbsoluteRoute(ApiRoutes.Login),
                new LoginCredentialsDto
                {
                    UsernameOrEmail = Email,
                    Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                });

            if (await result.HandleErrorIfFailedAsync("Login Failed", _settingsViewModel, _uiManager) || result.ServerResponse.Response is null)
                return;

            await _applicationViewModel.HandleSuccessfulLoginAsync(result.ServerResponse.Response);
        });
    }

    [ICommand]
    private void Register()
    {
        _applicationViewModel.GoToPage(ApplicationPage.Register);
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

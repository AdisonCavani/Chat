using Chat.ApiSDK;
using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.Db.Models.Entities;
using Chat.Extensions;
using Chat.Models;
using Chat.Services;
using Chat.Stores;
using Chat.Views;
using Chat.Views.Password;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Refit;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Chat.ViewModels;

[ObservableObject]
public partial class LoginViewModel : IDisposable
{
    private readonly Frame _frame;
    private readonly UserCredentialsManager _context;

    private readonly IAccountApi _accountApi;
    private readonly IProfileApi _profileApi;

    private readonly InfobarStore _infobarStore;
    private readonly CredentialsStore _credentialsStore;

    public LoginViewModel(
        Frame frame,
        UserCredentialsManager context,
        IAccountApi accountApi,
        IProfileApi profileApi,
        InfobarStore infobarStore,
        CredentialsStore credentialsStore)
    {
        _frame = frame;
        _context = context;

        _accountApi = accountApi;
        _profileApi = profileApi;

        _infobarStore = infobarStore;
        _infobarStore.InfobarUpdated += OnInfobarUpdated;

        _credentialsStore = credentialsStore;
        _credentialsStore.CredentialsUpdated += OnCredentialsUpdated;
    }

    void OnInfobarUpdated(Infobar infobar)
    {
        InfoTitle = new InfobarViewModel(infobar).Title;
        InfoMessage = new InfobarViewModel(infobar).Message;
        InfoSeverity = new InfobarViewModel(infobar).Severity;
        InfoVisible = new InfobarViewModel(infobar).Visible;
    }

    void OnCredentialsUpdated(Credentials credentials)
    {
        Email = new CredentialsViewModel(credentials).Email;
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExecute))]
    string email;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExecute))]
    string password;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExecute))]
    bool isRunning;

    public bool CanExecute =>
        !IsRunning &&
        Validators.IsEmailAdress(Email) &&
        !string.IsNullOrWhiteSpace(Password);

    #region Infobar
    [ObservableProperty]
    string infoTitle;

    [ObservableProperty]
    string infoMessage;

    [ObservableProperty]
    InfoBarSeverity infoSeverity;

    [ObservableProperty]
    bool infoVisible;
    #endregion

    [ICommand]
    async Task Login()
    {
        IsRunning = true;
        await HandleLogin();
        IsRunning = false;
    }

    [ICommand]
    void RecoverPassword()
    {
        _credentialsStore.UpdateCredentials(new()
        {
            Email = Email
        });


        _frame.Navigate(
                typeof(RecoverPage),
                null,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                });
    }

    [ICommand]
    void GoToRegisterPage()
    {
        _frame.Navigate(
                typeof(RegisterPage),
                null,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromLeft
                });
    }

    async Task HandleLogin()
    {
        LoginCredentialsDto dto = new()
        {
            Email = Email,
            Password = Password
        };

        try
        {
            var loginResult = await _accountApi.LoginAsync(dto);
            var profileResult = await _profileApi.GetUserDetailsAsync(loginResult.Token);

            UserCredentials credentials = new()
            {
                Email = profileResult.Email,
                FirstName = profileResult.FirstName,
                LastName = profileResult.LastName,
                Token = loginResult.Token
            };

            await HandleSuccess(credentials);
        }
        catch (ApiException ex)
        {
            HandleFailure(await ex.GetContentAsAsync<ErrorResponse>());
        }
        finally
        {
            IsRunning = false;
        }
    }

    void HandleFailure(ErrorResponse apiError)
    {
        StringBuilder sb = new();
        foreach (var error in apiError.Errors)
            sb.AppendLine(error);

        InfoTitle = "Login failed";
        InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        InfoSeverity = InfoBarSeverity.Error;
        InfoVisible = true;
    }

    async Task HandleSuccess(UserCredentials credentials)
    {
        var saved = await _context.SaveUserCredentials(credentials);

        if (!saved)
        {
            InfoTitle = "Login failed";
            InfoMessage = "Something went wrong during saving credentials";
            InfoSeverity = InfoBarSeverity.Error;
            InfoVisible = true;

            return;
        }

        _frame.Navigate(typeof(ChatPage));
        _credentialsStore.ClearCredentials();
        Password = string.Empty;
    }

    public void Dispose()
    {
        _infobarStore.InfobarUpdated -= OnInfobarUpdated;
        _credentialsStore.CredentialsUpdated -= OnCredentialsUpdated;
    }
}

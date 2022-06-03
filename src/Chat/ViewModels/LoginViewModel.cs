using Chat.Core;
using Chat.Core.Models.Entities;
using Chat.Core.Models.Requests;
using Chat.Core.Models.Responses;
using Chat.Db.Models.Entities;
using Chat.Extensions;
using Chat.Services;
using Chat.Views;
using Chat.Views.Password;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Chat.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly Frame _frame;
    private readonly UserCredentialsManager _context;

    public LoginViewModel(Frame frame, UserCredentialsManager context)
    {
        _frame = frame;
        _context = context;
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

    [ObservableProperty]
    bool infoVisible;

    [ObservableProperty]
    string infoTitle;

    [ObservableProperty]
    string infoMessage;

    [ObservableProperty]
    InfoBarSeverity infoSeverity;

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

        using HttpClient client = new();
        var loginCall = await client.PostAsJsonAsync($"https://localhost:5001/{ApiRoutes.Account.Login}", dto);

        var loginJson = await loginCall.Content.ReadAsStringAsync();
        var loginObj = JsonConvert.DeserializeObject<ApiResponse<JwtTokenDto>>(loginJson);

        if (!loginCall.IsSuccessStatusCode)
        {
            HandleFailure(loginObj);
            return;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginObj.Result.Token);
        var tokenCall = await client.GetAsync($"https://localhost:5001/{ApiRoutes.Account.Auth}");

        var tokenJson = await tokenCall.Content.ReadAsStringAsync();
        var tokenObj = JsonConvert.DeserializeObject<ApiResponse<UserProfile>>(tokenJson);

        if (!tokenCall.IsSuccessStatusCode)
        {
            HandleFailure(tokenObj);
            return;
        }

        UserCredentials credentials = new()
        {
            Email = tokenObj.Result.Email,
            FirstName = tokenObj.Result.FirstName,
            LastName = tokenObj.Result.LastName,
            Token = loginObj.Result.Token
        };

        await HandleSuccess(credentials);
    }

    void HandleFailure<T>(ApiResponse<T> response)
    {
        StringBuilder sb = new();
        foreach (var error in response?.Errors)
            sb.AppendLine(error);

        InfoTitle = "Login failed";
        InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        InfoSeverity = InfoBarSeverity.Error;
        InfoVisible = true;

        IsRunning = false;
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

            IsRunning = false;

            return;
        }

        IsRunning = false;
        _frame.Navigate(typeof(HubPage));
    }
}

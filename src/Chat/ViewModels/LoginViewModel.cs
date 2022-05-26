using Chat.Core;
using Chat.Core.Models.Entities;
using Chat.Core.Models.Requests;
using Chat.Db.Models.Entities;
using Chat.Extensions;
using Chat.Services;
using Chat.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    async void Login()
    {
        IsRunning = true;

        LoginCredentialsDto dto = new()
        {
            Email = Email,
            Password = Password
        };

        using HttpClient client = new();
        var response = await client.PostAsJsonAsync($"https://localhost:5001/{ApiRoutes.Account.Login}", dto);

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<ApiResponse<RefreshTokenDto>>(json);

        if (!response.IsSuccessStatusCode)
        {
            StringBuilder sb = new();
            foreach (var error in obj.Errors)
                sb.AppendLine(error);

            InfoTitle = "Login failed";
            InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            InfoSeverity = InfoBarSeverity.Error;
            InfoVisible = true;
            IsRunning = false;
            return;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", obj.Result.Token);
        var response2 = await client.GetAsync($"https://localhost:5001/{ApiRoutes.Account.Auth}");

        var json2 = await response2.Content.ReadAsStringAsync();
        var obj2 = JsonConvert.DeserializeObject<ApiResponse<UserProfile>>(json2);

        if (!response2.IsSuccessStatusCode)
        {
            StringBuilder sb = new();
            foreach (var error in obj2.Errors)
                sb.AppendLine(error);

            InfoTitle = "Login failed";
            InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            InfoSeverity = InfoBarSeverity.Error;
            InfoVisible = true;

            IsRunning = false;
            return;
        }

        UserCredentials credentials = new()
        {
            Email = obj2.Result.Email,
            FirstName = obj2.Result.FirstName,
            LastName = obj2.Result.LastName,
            Token = obj.Result.Token,
            RefreshToken = obj.Result.RefreshToken
        };

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

    [ICommand]
    void RecoverPassword()
    {
        _frame.Navigate(
                typeof(RecoverPasswordPage),
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
}

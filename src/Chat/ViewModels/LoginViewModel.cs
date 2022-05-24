using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.Extensions;
using Chat.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Chat.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanLogin))]
    string email;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanLogin))]
    string password;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanLogin))]
    bool loginRunning;

    public bool CanLogin => !LoginRunning && Validators.IsEmailAdress(Email) && !string.IsNullOrWhiteSpace(Password);

    [ObservableProperty]
    bool loginFailed;

    [ObservableProperty]
    string errorMessage;

    [ICommand]
    async void Login()
    {
        LoginRunning = true;

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

            ErrorMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            LoginFailed = true;
            LoginRunning = false;

            return;
        }

        // TODO: handle navigation
        LoginFailed = false;

        LoginRunning = false;
    }

    [ICommand]
    void RecoverPassword()
    {
        App.Current.Services.GetRequiredService<Frame>()
            .Navigate(
                typeof(RecoverPasswordPage),
                null,
                new SlideNavigationTransitionInfo()
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                });
    }
}

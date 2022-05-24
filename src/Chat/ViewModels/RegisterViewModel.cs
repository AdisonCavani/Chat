using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.Extensions;
using Chat.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Chat.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExecute))]
    string firstName;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExecute))]
    string lastName;

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
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(lastName) &&
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
    async void Register()
    {
        IsRunning = true;

        RegisterCredentialsDto dto = new()
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password
        };

        using HttpClient client = new();
        var response = await client.PostAsJsonAsync($"https://localhost:5001/{ApiRoutes.Account.Register}", dto);

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<ApiResponse>(json);

        if (!response.IsSuccessStatusCode)
        {
            StringBuilder sb = new();
            foreach (var error in obj.Errors)
                sb.AppendLine(error);

            InfoTitle = "Sign up failed";
            InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            InfoSeverity = InfoBarSeverity.Error;
            InfoVisible = true;

            IsRunning = false;
            return;
        }

        IsRunning = false;

        var loginVM = App.Current.Services.GetRequiredService<LoginViewModel>();

        // Infobar
        loginVM.InfoTitle = "Register successful";
        loginVM.InfoMessage = "Now you can login using your credentials";
        loginVM.InfoSeverity = InfoBarSeverity.Informational;
        loginVM.InfoVisible = true;

        loginVM.Email = Email;
        loginVM.Password = string.Empty;

        ClearCredentials();

        App.Current.Services.GetRequiredService<Frame>()
            .Navigate(typeof(LoginPage));
    }

    [ICommand]
    void GoToLoginPage()
    {
        App.Current.Services.GetRequiredService<Frame>()
            .Navigate(
                typeof(LoginPage),
                null,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                });
    }

    void ClearCredentials()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }
}

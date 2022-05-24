﻿using Chat.Core;
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

public partial class LoginViewModel : ObservableObject
{
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

        IsRunning = false;

        App.Current.Services.GetRequiredService<Frame>()
            .Navigate(typeof(HubPage));
    }

    [ICommand]
    void RecoverPassword()
    {
        App.Current.Services.GetRequiredService<Frame>()
            .Navigate(
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
        App.Current.Services.GetRequiredService<Frame>()
            .Navigate(
                typeof(RegisterPage),
                null,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromLeft
                });
    }
}

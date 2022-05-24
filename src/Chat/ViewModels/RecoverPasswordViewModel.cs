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

public partial class RecoverPasswordViewModel : ObservableObject
{
    public RecoverPasswordViewModel()
    {
        Email = App.Current.Services.GetService<LoginViewModel>().Email ?? string.Empty;
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanRecover))]
    string email;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanRecover))]
    bool recoveryRunning;

    [ObservableProperty]
    bool infoVisible;

    [ObservableProperty]
    string infoTitle;

    [ObservableProperty]
    string infoMessage;

    [ObservableProperty]
    InfoBarSeverity infoSeverity;

    public bool CanRecover => !RecoveryRunning && Validators.IsEmailAdress(Email);

    [ICommand]
    async void RecoverPassword()
    {
        RecoveryRunning = true;

        PasswordRecoveryDto dto = new()
        {
            Email = Email
        };

        using HttpClient client = new();
        var response = await client.PostAsJsonAsync($"https://localhost:5001/{ApiRoutes.Account.PasswordRecovery}", dto);

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<ApiResponse>(json);

        if (!response.IsSuccessStatusCode)
        {
            StringBuilder sb = new();
            foreach (var error in obj.Errors)
                sb.AppendLine(error);

            InfoTitle = "Recovery failed";
            InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            InfoSeverity = InfoBarSeverity.Error;
            InfoVisible = true;
            RecoveryRunning = false;

            return;
        }

        // TODO: handle navigation
        InfoTitle = "Recovery succeeded";
        InfoMessage = "We have sent a password recover instructions to your email";
        InfoSeverity = InfoBarSeverity.Success;
        InfoVisible = true;

        RecoveryRunning = false;
    }

    [ICommand]
    void GoBack()
    {
        var frame = App.Current.Services.GetRequiredService<Frame>();

        if (frame.CanGoBack)
            frame.GoBack();
    }
}

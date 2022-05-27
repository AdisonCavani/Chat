using Chat.Core;
using Chat.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Chat.ViewModels;

public partial class RecoverPasswordViewModel : ObservableObject
{
    public RecoverPasswordViewModel()
    {
        Email = App.Current.Services.GetService<LoginViewModel>().Email ?? string.Empty;

        InitComponents();
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExecute))]
    string email;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExecute))]
    bool isRunning;

    [ObservableProperty]
    bool infoVisible;

    [ObservableProperty]
    string infoTitle;

    [ObservableProperty]
    string infoMessage;

    [ObservableProperty]
    InfoBarSeverity infoSeverity;

    public bool CanExecute => !IsRunning && Validators.IsEmailAdress(Email);

    [ICommand]
    async Task SendEmail()
    {
        IsRunning = true;

        using HttpClient client = new();
        var response = await client.GetAsync($"https://localhost:5001/{ApiRoutes.Account.PasswordRecovery}?email={Email}");

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<ApiResponse>(json);

        if (!response.IsSuccessStatusCode)
        {
            HandleFailure(obj);
            return;
        }

        InfoTitle = "Recovery succeeded";
        InfoMessage = "We have sent a password recover instructions to your email";
        InfoSeverity = InfoBarSeverity.Success;
        InfoVisible = true;

        IsRunning = false;
    }

    [ICommand]
    void RecoverPassword()
    {

    }

    [ICommand]
    void GoBack()
    {
        var frame = App.Current.Services.GetRequiredService<Frame>();

        if (frame.CanGoBack)
            frame.GoBack();
    }

    void InitComponents()
    {
        InfoTitle = "Reset password";
        InfoMessage = "Enter the email associated with your accound and we'll send an email with instructions to reset your password";
        InfoSeverity = InfoBarSeverity.Informational;
        InfoVisible = true;
    }

    void HandleFailure(ApiResponse response)
    {
        StringBuilder sb = new();
        foreach (var error in response.Errors)
            sb.AppendLine(error);

        InfoTitle = "Login failed";
        InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        InfoSeverity = InfoBarSeverity.Error;
        InfoVisible = true;

        IsRunning = false;
    }
}

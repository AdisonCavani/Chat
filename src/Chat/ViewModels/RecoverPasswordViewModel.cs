using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.Core.Models.Response;
using Chat.Extensions;
using Chat.Views;
using Chat.Views.Password;
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
    private readonly Frame _frame;
    private readonly HttpClient _httpClient;

    public RecoverPasswordViewModel(Frame frame)
    {
        _frame = frame;
        _httpClient = new();

        InitComponents();
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    string email;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanVerify))]
    string token;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanChange))]
    string newPassword;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    [AlsoNotifyChangeFor(nameof(CanVerify))]
    [AlsoNotifyChangeFor(nameof(CanChange))]
    bool isRunning;

    [ObservableProperty]
    bool infoVisible;

    [ObservableProperty]
    string infoTitle;

    [ObservableProperty]
    string infoMessage;

    [ObservableProperty]
    InfoBarSeverity infoSeverity;

    public bool CanSend => !IsRunning && Validators.IsEmailAdress(Email);

    public bool CanVerify => !IsRunning && !string.IsNullOrWhiteSpace(Token);

    public bool CanChange => !IsRunning && !string.IsNullOrWhiteSpace(NewPassword);

    [ICommand]
    async Task SendEmail()
    {
        IsRunning = true;

        var response = await _httpClient.GetAsync($"https://localhost:5001/{ApiRoutes.Account.Password.SendRecoveryEmail}?email={Email}");

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<ApiResponse>(json);

        if (!response.IsSuccessStatusCode)
        {
            HandleFailure(obj);
            return;
        }

        _frame.Navigate(typeof(TokenPage));

        InfoTitle = "Email send";
        InfoMessage = "We have sent you a password recovery token to your email";
        InfoSeverity = InfoBarSeverity.Success;
        InfoVisible = true;

        IsRunning = false;
    }

    [ICommand]
    async Task VerifyToken()
    {
        IsRunning = true;

        PasswordRecoveryTokenDto dto = new()
        {
            Email = Email,
            Token = Token
        };

        var response = await _httpClient.PostAsJsonAsync($"https://localhost:5001/{ApiRoutes.Account.Password.VerifyToken}", dto);

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<ApiResponse<TempTokenDto>>(json);

        if (!response.IsSuccessStatusCode)
        {
            HandleFailure(obj);
            return;
        }

        _frame.Navigate(typeof(ChangePage));

        InfoTitle = "Token verified";
        InfoMessage = "Token has been verified. Now you can set new password";
        InfoSeverity = InfoBarSeverity.Success;
        InfoVisible = true;

        IsRunning = false;
    }

    [ICommand]
    async Task ChangePassword()
    {
        IsRunning = true;

        ResetPasswordDto dto = new()
        {
            Email = Email,
            NewPassword = NewPassword,
            Token = Token
        };

        var response = await _httpClient.PostAsJsonAsync($"https://localhost:5001/{ApiRoutes.Account.Password.Reset}", dto);

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<ApiResponse>(json);

        if (!response.IsSuccessStatusCode)
        {
            HandleFailure(obj);
            return;
        }

        var vm = App.Current.Services.GetRequiredService<LoginViewModel>();

        _frame.Navigate(typeof(LoginPage));

        vm.InfoTitle = "Password changed";
        vm.InfoMessage = "Now you can login with your new credentials";
        vm.InfoSeverity = InfoBarSeverity.Success;
        vm.InfoVisible = true;

        IsRunning = false;
    }

    [ICommand]
    void GoBack()
    {
        if (_frame.CanGoBack)
            _frame.GoBack();
    }

    void InitComponents()
    {
        Email = App.Current.Services.GetService<LoginViewModel>().Email ?? string.Empty;

        InfoTitle = "Reset password";
        InfoMessage = "Enter the email associated with your accound and we'll send an email with instructions to reset your password";
        InfoSeverity = InfoBarSeverity.Informational;
        InfoVisible = true;
    }

    // TODO: resolve this duplication
    void HandleFailure(ApiResponse response)
    {
        StringBuilder sb = new();
        foreach (var error in response.Errors)
            sb.AppendLine(error);

        InfoTitle = "Something went wrong";
        InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        InfoSeverity = InfoBarSeverity.Error;
        InfoVisible = true;

        IsRunning = false;
    }

    void HandleFailure<T>(ApiResponse<T> response)
    {
        StringBuilder sb = new();
        foreach (var error in response.Errors)
            sb.AppendLine(error);

        InfoTitle = "Something went wrong";
        InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        InfoSeverity = InfoBarSeverity.Error;
        InfoVisible = true;

        IsRunning = false;
    }
}

using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.Extensions;
using Chat.Models;
using Chat.Stores;
using Chat.Views;
using Chat.Views.Password;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Chat.ViewModels;

[ObservableObject]
public partial class RecoverPasswordViewModel : IDisposable
{
    private readonly Frame _frame;
    private readonly HttpClient _httpClient;

    private readonly InfobarStore _infobarStore;
    private readonly CredentialsStore _credentialsStore;

    public RecoverPasswordViewModel(Frame frame, InfobarStore infobarStore, CredentialsStore credentialsStore)
    {
        _frame = frame;
        _httpClient = new();

        _infobarStore = infobarStore;

        _credentialsStore = credentialsStore;
        _credentialsStore.CredentialsUpdated += OnCredentialsUpdated;

        InitComponents();
    }

    void OnCredentialsUpdated(Credentials credentials)
    {
        Email = new CredentialsViewModel(credentials).Email;
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
    string infoTitle;

    [ObservableProperty]
    string infoMessage;

    [ObservableProperty]
    InfoBarSeverity infoSeverity;

    [ObservableProperty]
    bool infoVisible;

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
        var obj = JsonConvert.DeserializeObject<ApiResponse>(json);

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

        _frame.Navigate(typeof(LoginPage));

        _infobarStore.UpdateInfobar(new()
        {
            Title = "Password changed",
            Message = "Now you can login using your credentials",
            Severity = InfoBarSeverity.Success,
            Visible = true
        });

        IsRunning = false;
    }

    [ICommand]
    void GoBack()
    {
        _credentialsStore.UpdateCredentials(new()
        {
            Email = Email
        });

        if (_frame.CanGoBack)
            _frame.GoBack();
    }

    void InitComponents()
    {
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

    public void Dispose()
    {
        _credentialsStore.CredentialsUpdated -= OnCredentialsUpdated;
    }
}

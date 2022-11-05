using Chat.ApiSDK;
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
using Refit;
using System;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Chat.ViewModels;

[ObservableObject]
public partial class RecoverPasswordViewModel : IDisposable
{
    private readonly Frame _frame;

    private readonly InfobarStore _infobarStore;

    private readonly IPasswordApi _passwordApi;

    private readonly CredentialsStore _credentialsStore;

    public RecoverPasswordViewModel(Frame frame, InfobarStore infobarStore, IPasswordApi passwordApi, CredentialsStore credentialsStore)
    {
        _frame = frame;

        _infobarStore = infobarStore;

        _passwordApi = passwordApi;

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

        var dto = new PasswordRecoveryDto()
        {
            Email = Email
        };

        try
        {
            await _passwordApi.SendRecoveryEmailAsync(dto);

            _frame.Navigate(typeof(TokenPage));

            InfoTitle = "Email send";
            InfoMessage = "We have sent you a password recovery token to your email";
            InfoSeverity = InfoBarSeverity.Success;
            InfoVisible = true;
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

    [ICommand]
    async Task VerifyToken()
    {
        IsRunning = true;

        PasswordRecoveryTokenDto dto = new()
        {
            Email = Email,
            Token = Token
        };

        try
        {
            await _passwordApi.VerifyTokenAsync(dto);

            _frame.Navigate(typeof(ChangePage));

            InfoTitle = "Token verified";
            InfoMessage = "Token has been verified. Now you can set new password";
            InfoSeverity = InfoBarSeverity.Success;
            InfoVisible = true;
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

        try
        {
            await _passwordApi.ResetAsync(dto);

            _frame.Navigate(typeof(LoginPage));

            _infobarStore.UpdateInfobar(new()
            {
                Title = "Password changed",
                Message = "Now you can login using your credentials",
                Severity = InfoBarSeverity.Success,
                Visible = true
            });
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
    void HandleFailure(ErrorResponse apiError)
    {
        StringBuilder sb = new();
        foreach (var error in apiError.Errors)
            sb.AppendLine(error);

        InfoTitle = "Something went wrong";
        InfoMessage = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        InfoSeverity = InfoBarSeverity.Error;
        InfoVisible = true;
    }

    public void Dispose()
    {
        _credentialsStore.CredentialsUpdated -= OnCredentialsUpdated;
    }
}

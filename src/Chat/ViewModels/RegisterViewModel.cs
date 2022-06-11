using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.Extensions;
using Chat.Stores;
using Chat.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Chat.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly Frame _frame;

    private readonly InfobarStore _infobarStore;

    public RegisterViewModel(Frame frame, InfobarStore infobarStore)
    {
        _frame = frame;

        _infobarStore = infobarStore;
    }

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

    #region Infobar
    [ObservableProperty]
    string infoTitle;

    [ObservableProperty]
    string infoMessage;

    [ObservableProperty]
    InfoBarSeverity infoSeverity;

    [ObservableProperty]
    bool infoVisible;
    #endregion

    [ICommand]
    async Task Register()
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

        _frame.Navigate(typeof(LoginPage));

        _infobarStore.UpdateInfobar(new()
        {
            Title = "Register successful",
            Message = "Now you can login using your credentials",
            Severity = InfoBarSeverity.Informational,
            Visible = true
        });

        ClearCredentials();
        IsRunning = false;
    }

    [ICommand]
    void GoToLoginPage()
    {
        _frame.Navigate(
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

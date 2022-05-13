using System;
using System.Linq.Expressions;
using System.Security;
using System.Threading.Tasks;
using Chat.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chat.ViewModels.Input;

public partial class PasswordEntryViewModel : ObservableObject
{
    [ObservableProperty]
    private string? label;

    [ObservableProperty]
    private string? fakePassword;

    [ObservableProperty]
    private string? currentPasswordHintText;

    [ObservableProperty]
    private string? newPasswordHintText;

    [ObservableProperty]
    private string? confirmPasswordHintText;

    [ObservableProperty]
    private SecureString? currentPassword;

    [ObservableProperty]
    private SecureString? newPassword;

    [ObservableProperty]
    private SecureString? confirmPassword;

    [ObservableProperty]
    private bool editing;

    [ObservableProperty]
    private bool working;

    [ObservableProperty]
    private Func<Task<bool>>? commitAction;

    public PasswordEntryViewModel()
    {
        // Set default hints
        // TODO: Replace with localization text
        CurrentPasswordHintText = "Current Password";
        NewPasswordHintText = "New Password";
        ConfirmPasswordHintText = "Confirm Password";
    }

    [ICommand]
    public void Edit()
    {
        // Clear all password
        NewPassword = new SecureString();
        ConfirmPassword = new SecureString();

        // Go into edit mode
        Editing = true;
    }
    [ICommand]
    public void Cancel()
    {
        Editing = false;
    }

    [ICommand]
    public void Save()
    {
        // Store the result of a commit call
        var result = default(bool);

        RunCommandAsync(() => Working, async () =>
        {
            // While working, come out of edit mode
            Editing = false;

            // Try and do the work
            result = CommitAction is null || await CommitAction();

        }).ContinueWith(_ =>
        {
            // If we succeeded...
            // Nothing to do
            // If we fail...
            if (!result)
            {
                Editing = true; // Go back into edit mode
            }
        });
    }

    // TODO: remove legacy BaseViewModel helpers
    private readonly object mPropertyValueCheckLock = new();

    private async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
    {
        lock (mPropertyValueCheckLock)
        {
            if (updatingFlag.GetPropertyValue())
                return;

            updatingFlag.SetPropertyValue(true);
        }

        try
        {
            await action();
        }
        finally
        {
            updatingFlag.SetPropertyValue(false);
        }
    }
}

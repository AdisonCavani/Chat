using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Core.ApiModels;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.DataModels;
using Chat.Core.DI.Interfaces;
using Chat.Core.Extensions;
using Chat.Core.Routes;
using Chat.ViewModel.Dialogs;
using Chat.ViewModel.Input;
using Chat.WebRequests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dna;
using static Chat.DI.DI;
using static Dna.FrameworkDI;

namespace Chat.ViewModel.Application;

public partial class SettingsViewModel : ObservableObject
{
    private const string mLoadingText = "...";

    [ObservableProperty]
    private TextEntryViewModel firstName;

    [ObservableProperty]
    private TextEntryViewModel lastName;

    [ObservableProperty]
    private TextEntryViewModel username;

    [ObservableProperty]
    private PasswordEntryViewModel password;

    [ObservableProperty]
    private TextEntryViewModel email;

    [ObservableProperty]
    private string logoutButtonText;

    [ObservableProperty]
    private bool firstNameIsSaving;

    [ObservableProperty]
    private bool lastNameIsSaving;

    [ObservableProperty]
    private bool usernameIsSaving;

    [ObservableProperty]
    private bool emailIsSaving;

    [ObservableProperty]
    private bool passwordIsChanging;

    [ObservableProperty]
    private bool settingsLoading;

    [ObservableProperty]
    private bool loggingOut;

    public ICommand SaveFirstNameCommand { get; set; }

    public ICommand SaveLastNameCommand { get; set; }

    public ICommand SaveUsernameCommand { get; set; }

    public ICommand SaveEmailCommand { get; set; }

    public SettingsViewModel()
    {
        // Create First Name
        FirstName = new TextEntryViewModel
        {
            Label = "First Name",
            OriginalText = mLoadingText,
            CommitAction = SaveFirstNameAsync
        };

        // Create Last Name
        LastName = new TextEntryViewModel
        {
            Label = "Last Name",
            OriginalText = mLoadingText,
            CommitAction = SaveLastNameAsync
        };

        // Create Username
        Username = new TextEntryViewModel
        {
            Label = "Username",
            OriginalText = mLoadingText,
            CommitAction = SaveUsernameAsync
        };

        // Create Password
        Password = new PasswordEntryViewModel
        {
            Label = "Password",
            FakePassword = "********",
            CommitAction = SavePasswordAsync
        };

        // Create Email
        Email = new TextEntryViewModel
        {
            Label = "Email",
            OriginalText = mLoadingText,
            CommitAction = SaveEmailAsync
        };

        // Create commands
        SaveFirstNameCommand = new RelayCommand(async () => await SaveFirstNameAsync());
        SaveLastNameCommand = new RelayCommand(async () => await SaveLastNameAsync());
        SaveUsernameCommand = new RelayCommand(async () => await SaveUsernameAsync());
        SaveEmailCommand = new RelayCommand(async () => await SaveEmailAsync());

        // TODO: Get from localization
        LogoutButtonText = "Logout";
    }

    [ICommand]
    public static void Open()
    {
        ViewModelApplication.SettingsMenuVisible = true;
    }

    [ICommand]
    public static void Close()
    {
        ViewModelApplication.SettingsMenuVisible = false;
    }

    [ICommand]
    public async Task Logout()
    {
        // Lock this command to ignore any other requests while processing
        await RunCommandAsync(() => LoggingOut, async () =>
        {
            // TODO: Confirm the user wants to logout

            // Clear any user data/cache
            await ClientDataStore.ClearAllLoginCredentialsAsync();

            // Clean all application level view models that contain
            // any information about the current user
            ClearUserData();

            // Go to login page
            ViewModelApplication.GoToPage(ApplicationPage.Login);
        });
    }

    [ICommand]
    public void ClearUserData()
    {
        // Clear all view models containing the users info
        FirstName.OriginalText = mLoadingText;
        LastName.OriginalText = mLoadingText;
        Username.OriginalText = mLoadingText;
        Email.OriginalText = mLoadingText;
    }

    [ICommand]
    public async Task Load()
    {
        // Lock this command to ignore any other requests while processing
        await RunCommandAsync(() => SettingsLoading, async () =>
        {
            // Store single transcient instance of client data store
            var scopedClientDataStore = ClientDataStore;

            // Update values from local cache
            await UpdateValuesFromLocalStoreAsync(scopedClientDataStore);

            // Get the user token
            var token = (await scopedClientDataStore.GetLoginCredentialsAsync())?.Token;

            // If we don't have a token (so we are not logged in...)
            if (string.IsNullOrEmpty(token))
                // Then do nothing more
                return;

            // Load user profile details form server
            var result = await Dna.WebRequests.PostAsync<ApiResponse<UserProfileDetailsDto>>(
                // Set URL
                RouteHelpers.GetAbsoluteRoute(ApiRoutes.GetUserProfile),
                // Pass in user Token
                bearerToken: token);

            // If the response has an error...
            if (await result.HandleErrorIfFailedAsync("Load User Details Failed"))
                // We are done
                return;

            // TODO: Should we check if the values are different before saving?

            // Create data model from the response
            var dataModel = result.ServerResponse.Response;

            // Re-add our known token
            dataModel.Token = token;

            // Save the new information in the data store
            await scopedClientDataStore.SaveLoginCredentialsAsync(dataModel);

            // Update values from local cache
            await UpdateValuesFromLocalStoreAsync(scopedClientDataStore);
        });
    }

    public async Task<bool> SaveFirstNameAsync()
    {
        // Lock this command to ignore any other requests while processing
        return await RunCommandAsync(() => FirstNameIsSaving, async () =>
        {
            // Update the First Name value on the server...
            return await UpdateUserCredentialsValueAsync(
                // Display name
                "First Name",
                // Update the first name
                (credentials) => credentials.FirstName,
                // To new value
                FirstName.OriginalText,
                // Set Api model value
                (apiModel, value) => apiModel.FirstName = value
                );
        });
    }

    public async Task<bool> SaveLastNameAsync()
    {
        // Lock this command to ignore any other requests while processing
        return await RunCommandAsync(() => LastNameIsSaving, async () =>
        {
            // Update the Last Name value on the server...
            return await UpdateUserCredentialsValueAsync(
                // Display name
                "Last Name",
                // Update the last name
                (credentials) => credentials.LastName,
                // To new value
                LastName.OriginalText,
                // Set Api model value
                (apiModel, value) => apiModel.LastName = value
                );
        });
    }

    public async Task<bool> SaveUsernameAsync()
    {
        // Lock this command to ignore any other requests while processing
        return await RunCommandAsync(() => UsernameIsSaving, async () =>
        {
            // Update the Username value on the server...
            return await UpdateUserCredentialsValueAsync(
                // Display name
                "Username",
                // Update the first name
                (credentials) => credentials.Username,
                // To new value
                Username.OriginalText,
                // Set Api model value
                (apiModel, value) => apiModel.Username = value
                );
        });
    }

    public async Task<bool> SaveEmailAsync()
    {
        // Lock this command to ignore any other requests while processing
        return await RunCommandAsync(() => EmailIsSaving, async () =>
        {
            // Update the Email value on the server...
            return await UpdateUserCredentialsValueAsync(
                // Display name
                "Email",
                // Update the email
                (credentials) => credentials.Email,
                // To new value
                Email.OriginalText,
                // Set Api model value
                (apiModel, value) => apiModel.Email = value
                );
        });
    }

    public async Task<bool> SavePasswordAsync()
    {
        // Lock this command to ignore any other requests while processing
        return await RunCommandAsync(() => PasswordIsChanging, async () =>
        {
            // Log it
            Logger.LogDebugSource($"Changing password...");

            // Get the current known credentials
            var credentials = await ClientDataStore.GetLoginCredentialsAsync();

            // Make sure the user has entered the same password
            if (Password.NewPassword.Unsecure() != Password.ConfirmPassword.Unsecure())
            {
                // Display error
                await UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    // TODO: Localize
                    Title = "Password Mismatch",
                    Message = "New password and confirm password must match"
                });

                // Return fail
                return false;
            }

            // Update the server with the new password
            var result = await Dna.WebRequests.PostAsync<ApiResponse>(
                // Set URL
                RouteHelpers.GetAbsoluteRoute(ApiRoutes.UpdateUserPassword),
                // Create API model
                new UpdateUserPasswordDto
                {
                    CurrentPassword = Password.CurrentPassword.Unsecure(),
                    NewPassword = Password.NewPassword.Unsecure()
                },
                // Pass in user Token
                bearerToken: credentials.Token);

            // If the response has an error...
            if (await result.HandleErrorIfFailedAsync($"Change Password"))
            {
                // Log it
                Logger.LogDebugSource($"Failed to change password. {result.ErrorMessage}");

                // Return false
                return false;
            }

            // Otherwise, we succeeded...

            // Log it
            Logger.LogDebugSource($"Successfully changed password");

            // Return successful
            return true;
        });
    }

    private async Task UpdateValuesFromLocalStoreAsync(IClientDataStore clientDataStore)
    {
        // Get the stored credentials
        var storedCredentials = await clientDataStore.GetLoginCredentialsAsync();

        // Set first name
        FirstName.OriginalText = storedCredentials?.FirstName ?? string.Empty;

        // Set last name
        LastName.OriginalText = storedCredentials?.LastName ?? string.Empty;

        // Set username
        Username.OriginalText = storedCredentials?.Username ?? string.Empty;

        // Set email
        Email.OriginalText = storedCredentials?.Email ?? string.Empty;
    }

    private static async Task<bool> UpdateUserCredentialsValueAsync(string displayName, Expression<Func<UserProfileDetailsDto, string>> propertyToUpdate, string newValue, Action<UpdateUserProfileDto, string> setApiModel)
    {
        // Log it
        Logger.LogDebugSource($"Saving {displayName}...");

        // Get the current known credentials
        var credentials = await ClientDataStore.GetLoginCredentialsAsync();

        // Get the property to update from the credentials
        var toUpdate = propertyToUpdate.GetPropertyValue(credentials);

        // Log it
        Logger.LogDebugSource($"{displayName} currently {toUpdate}, updating to {newValue}");

        // Check if the value is the same. If so...
        if (toUpdate == newValue)
        {
            // Log it
            Logger.LogDebugSource($"{displayName} is the same, ignoring");

            // Return true
            return true;
        }

        // Set the property
        propertyToUpdate.SetPropertyValue(newValue, credentials);

        // Create update details
        var updateApiModel = new UpdateUserProfileDto();

        // Ask caller to set appropriate value
        setApiModel(updateApiModel, newValue);

        // Update the server with the details
        var result = await Dna.WebRequests.PostAsync<ApiResponse>(
            // Set URL
            RouteHelpers.GetAbsoluteRoute(ApiRoutes.UpdateUserProfile),
            // Pass the Api model
            updateApiModel,
            // Pass in user Token
            bearerToken: credentials.Token);

        // If the response has an error...
        if (await result.HandleErrorIfFailedAsync($"Update {displayName}"))
        {
            // Log it
            Logger.LogDebugSource($"Failed to update {displayName}. {result.ErrorMessage}");

            // Return false
            return false;
        }

        // Log it
        Logger.LogDebugSource($"Successfully updated {displayName}. Saving to local database cache...");

        // Store the new user credentials the data store
        await ClientDataStore.SaveLoginCredentialsAsync(credentials);

        // Return successful
        return true;
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

    private async Task<T?> RunCommandAsync<T>(Expression<Func<bool>> updatingFlag, Func<Task<T>> action, T? defaultValue = default)
    {
        lock (mPropertyValueCheckLock)
        {
            if (updatingFlag.GetPropertyValue())
                return defaultValue;

            updatingFlag.SetPropertyValue(true);
        }

        try
        {
            return await action();
        }
        finally
        {
            updatingFlag.SetPropertyValue(false);
        }
    }
}

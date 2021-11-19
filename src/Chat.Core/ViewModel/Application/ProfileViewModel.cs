﻿using System.Threading.Tasks;
using System.Windows.Input;

namespace Chat.Core;

/// <summary>
/// The profile state as a view model
/// </summary>
public class ProfileViewModel : BaseViewModel
{
    #region Public Properties

    /// <summary>
    /// The current users name
    /// </summary>
    public TextEntryViewModel Name { get; set; }

    /// <summary>
    /// The current users username
    /// </summary>
    public TextEntryViewModel Username { get; set; }

    /// <summary>
    /// The current users password
    /// </summary>
    public PasswordEntryViewModel Password { get; set; }

    /// <summary>
    /// The current users email
    /// </summary>
    public TextEntryViewModel Email { get; set; }

    /// <summary>
    /// The text for the logout button
    /// </summary>
    public string LogoutButtonText { get; set; }

    #endregion

    #region Public Commands

    /// <summary>
    /// The command to open the profile menu
    /// </summary>
    public ICommand OpenCommand { get; set; }

    /// <summary>
    /// The command to close the profile menu
    /// </summary>
    public ICommand CloseCommand { get; set; }

    /// <summary>
    /// The command to logout of the application
    /// </summary>
    public ICommand LogoutCommand { get; set; }

    /// <summary>
    /// The command to clear the users data from the view model
    /// </summary>
    public ICommand ClearUserDataCommand { get; set; }

    /// <summary>
    /// The command to load settings data from the client data store
    /// </summary>
    public ICommand LoadCommand { get; set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProfileViewModel()
    {
        // Create commands
        OpenCommand = new RelayCommand(Open);
        CloseCommand = new RelayCommand(Close);
        LogoutCommand = new RelayCommand(Logout);
        ClearUserDataCommand = new RelayCommand(ClearUserData);
        LoadCommand = new RelayCommand(async () => await LoadAsync());

        // TODO: Get from localization
        LogoutButtonText = "Logout";
    }

    #endregion

    /// <summary>
    /// Closes the profile menu
    /// </summary>
    public void Open()
    {
        // Open the profile menu
        IoC.Application.ProfileMenuVisible = true;
    }

    /// <summary>
    /// Closes the profile menu
    /// </summary>
    public void Close()
    {
        // Close the profile menu
        IoC.Application.ProfileMenuVisible = false;
    }

    /// <summary>
    /// Logs the user out
    /// </summary>
    public void Logout()
    {
        // TODO: Confirm the user wants to logout

        // TODO: Clear any user data/cache

        // Clean all application level view models that contain
        // any information about the current user
        ClearUserData();

        // Go to login page
        IoC.Application.GoToPage(ApplicationPage.Login);
    }

    /// <summary>
    /// Clears any data specific to the current user
    /// </summary>
    public void ClearUserData()
    {
        // Clear all view models containing the users info
        Name = null;
        Username = null;
        Password = null;
        Email = null;
    }

    /// <summary>
    /// Sets the profile view model properties based on the data in the client data store
    /// </summary>
    public async Task LoadAsync()
    {
        // Get the stored credentials
        var storedCredentials = await IoC.ClientDataStore.GetLoginCredentialsAsync();

        Name = new TextEntryViewModel { Label = "Name", OriginalText = $"{storedCredentials?.FirstName} {storedCredentials?.LastName}" };
        Username = new TextEntryViewModel { Label = "Username", OriginalText = storedCredentials?.Username };
        Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
        Email = new TextEntryViewModel { Label = "Email", OriginalText = storedCredentials?.Email };
    }
}

using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Core;
using static Chat.DependencyInjection;

namespace Chat;

/// <summary>
/// The profile state as a view model
/// </summary>
public class ProfileViewModel : BaseViewModel
{
    #region Public Properties

    /// <summary>
    /// The current users first name
    /// </summary>
    public TextEntryViewModel FirstName { get; set; }

    /// <summary>
    /// The current users last name
    /// </summary>
    public TextEntryViewModel? LastName { get; set; }

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
        ViewModelApplication.ProfileMenuVisible = true;
    }

    /// <summary>
    /// Closes the profile menu
    /// </summary>
    public void Close()
    {
        // Close the profile menu
        ViewModelApplication.ProfileMenuVisible = false;
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
        ViewModelApplication.GoToPage(ApplicationPage.Login);
    }

    /// <summary>
    /// Clears any data specific to the current user
    /// </summary>
    public void ClearUserData()
    {
        // Clear all view models containing the users info
        FirstName = null;
        LastName = null;
        Password = null;
        Email = null;
    }

    /// <summary>
    /// Sets the profile view model properties based on the data in the client data store
    /// </summary>
    public async Task LoadAsync()
    {
        // Get the stored credentials
        var storedCredentials = await ViewModelClientDataStore.GetLoginCredentialsAsync();

        FirstName = new TextEntryViewModel { Label = "First name", TagText = "First name", OriginalText = $"{storedCredentials.FirstName}" };
        LastName = new TextEntryViewModel { Label = "Last name", TagText = "Last name", OriginalText = $"{storedCredentials?.LastName}" };
        Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
        Email = new TextEntryViewModel { Label = "Email", TagText = "Email", OriginalText = storedCredentials.Email };
    }
}

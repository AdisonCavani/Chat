using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using Dna;

namespace Chat.Core;

/// <summary>
/// The View Model for a login screen
/// </summary>
public class LoginViewModel : BaseViewModel
{
    #region Public Properties

    /// <summary>
    /// The email of the user
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// A flag indicating if the login command is running
    /// </summary>
    public bool LoginIsRunning { get; set; }

    #endregion

    #region Commands

    /// <summary>
    /// The command to login
    /// </summary>
    public ICommand LoginCommand { get; set; }

    /// <summary>
    /// The command to register
    /// </summary>
    public ICommand RegisterCommand { get; set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public LoginViewModel()
    {
        // Create commands
        LoginCommand = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
        RegisterCommand = new RelayCommand(async () => await RegisterAsync());
    }

    #endregion

    /// <summary>
    /// Attempts to log the user in
    /// </summary>
    /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
    /// <returns></returns>
    private async Task LoginAsync(object parameter)
    {
        await RunCommandAsync(() => LoginIsRunning, async () =>
        {
            // TODO: Move all URLs and API routes to static class in core
            // Call the server and attempt to login with credentials
            var result = await WebRequests.PostAsync<ApiResponse<LoginResultApiModel>>(
            "https://localhost:7283/api/login",
            new LoginCredentialsApiModel
            {
                UsernameOrEmail = Email,
                Password = (parameter as IHavePassword).SecurePassword.Unsecure()
            });

            // If there was no response, bad data or a response with a error message
            if (result is null || result.ServerResponse is null || !result.ServerResponse.Successful)
            {
                // TODO: Localize strings
                // Default error message
                var message = "Unknown error from server call";

                // If we got a response from the server
                if (result?.ServerResponse is not null)
                    // Set message to server response
                    message = result.ServerResponse.ErrorMessage;

                // If we have a result, but deserialize failed
                else if (string.IsNullOrWhiteSpace(result?.RawServerResponse))
                    // Set error message
                    message = $"Unexpected response from server. {result.RawServerResponse}";

                // If we have a result, but no server response details at all
                else if (result is not null)
                    // Set message to standard HTTP server response details
                    message = $"Failed to communicate with server. Status code: {result.StatusCode}. {result.StatusDescription}";

                // Display error
                await IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    // TODO: Localize strings
                    Title = "Login Failed",
                    Message = message
                });

                // We are done
                return;
            }

            // OK successfully logged in... now get users data
            var userData = result.ServerResponse.Response;

            IoC.Profile.Name = new TextEntryViewModel { Label = "Name", OriginalText = $"{userData.FirstName} {userData.LastName}" };
            IoC.Profile.Username = new TextEntryViewModel { Label = "Username", OriginalText = userData.Username };
            IoC.Profile.Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
            IoC.Profile.Email = new TextEntryViewModel { Label = "Email", OriginalText = userData.Email };

            // Go to chat page
            IoC.Application.GoToPage(ApplicationPage.Chat);
        });
    }

    /// <summary>
    /// Takes the user to the register page
    /// </summary>
    /// <returns></returns>
    private async Task RegisterAsync()
    {
        // Go to register page
        IoC.Application.GoToPage(ApplicationPage.Register);
    }
}

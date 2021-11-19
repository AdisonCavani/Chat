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
                Email = Email,
                Password = (parameter as IHavePassword).SecurePassword.Unsecure()
            });

            // Response has an error
            if (await result.DisplayErrorIfFailedAsync("Login failed"))
                return;

            // OK successfully logged in... now get users data
            var loginResult = result.ServerResponse.Response;

            // Let the application view model handle what happens
            // with the successful login
            await IoC.Application.HandleSuccessfulLoginAsync(loginResult);
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

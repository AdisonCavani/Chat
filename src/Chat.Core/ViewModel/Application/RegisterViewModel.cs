using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using Dna;

namespace Chat.Core;

/// <summary>
/// The View Model for a register screen
/// </summary>
public class RegisterViewModel : BaseViewModel
{
    #region Public Properties

    /// <summary>
    /// The email of the user
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// A flag indicating if the register command is running
    /// </summary>
    public bool RegisterIsRunning { get; set; }

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
    public RegisterViewModel()
    {
        // Create commands
        RegisterCommand = new RelayParameterizedCommand(async (parameter) => await RegisterAsync(parameter));
        LoginCommand = new RelayCommand(async () => await LoginAsync());
    }

    #endregion

    /// <summary>
    /// Attempts to register a new user
    /// </summary>
    /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
    /// <returns></returns>
    private async Task RegisterAsync(object parameter)
    {
        await RunCommandAsync(() => RegisterIsRunning, async () =>
        {
            // TODO: Move all URLs and API routes to static class in core
            // Call the server and attempt to register with provided credentials
            var result = await WebRequests.PostAsync<ApiResponse<RegisterResultApiModel>>(
            "https://localhost:7283/api/register",
            new RegisterCredentialsApiModel
            {
                Email = Email,
                FirstName = "siema",
                LastName = string.Empty,
                Password = (parameter as IHavePassword).SecurePassword.Unsecure(),
            });

            // Response has an error
            if (await result.DisplayErrorIfFailedAsync("Register failed"))
                return;

            // OK successfully registered and logged in... now get users data
            var loginResult = result.ServerResponse.Response;

            // Let the application view model handle what happens
            // with the successful login
            await IoC.Application.HandleSuccessfulLoginAsync(loginResult);
        });
    }

    /// <summary>
    /// Takes the user to the login page
    /// </summary>
    /// <returns></returns>
    private async Task LoginAsync()
    {
        // Go to login page
        IoC.Application.GoToPage(ApplicationPage.Login);
    }
}
namespace Chat.Core;

/// <summary>
/// The credentials for an API client to log into the server and receive token back
/// </summary>
public class LoginCredentialsApiModel
{
    #region Public Properties

    /// <summary>
    /// The users email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The users password
    /// </summary>
    public string Password { get; set; }

    #endregion

    public LoginCredentialsApiModel()
    {

    }
}

namespace Chat.Core;

/// <summary>
/// The credentials for an API client to register on the server
/// </summary>
public class RegisterCredentialsApiModel
{
    #region Public Properties

    /// <summary>
    /// The users username or email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The users username or email
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The users username or email
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// The users password
    /// </summary>
    public string Password { get; set; }

    #endregion

    public RegisterCredentialsApiModel()
    {

    }
}

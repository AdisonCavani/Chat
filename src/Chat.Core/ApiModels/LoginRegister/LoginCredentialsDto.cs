namespace Chat.Core.ApiModels.LoginRegister;

/// <summary>
/// The credentials for an API client to log into the server and receive a token back
/// </summary>
public class LoginCredentialsDto
{
    /// <summary>
    /// The users username or email
    /// </summary>
    public string? UsernameOrEmail { get; init; }

    /// <summary>
    /// The users password
    /// </summary>
    public string? Password { get; init; }
}

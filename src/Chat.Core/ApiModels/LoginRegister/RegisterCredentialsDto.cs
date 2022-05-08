namespace Chat.Core.ApiModels.LoginRegister;

/// <summary>
/// The credentials for an API client to register on the server
/// </summary>
public class RegisterCredentialsDto
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }
}

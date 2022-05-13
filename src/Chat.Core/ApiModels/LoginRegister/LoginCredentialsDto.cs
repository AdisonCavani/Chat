namespace Chat.Core.ApiModels.LoginRegister;

public class LoginCredentialsDto
{
    public string? UsernameOrEmail { get; init; }

    public string? Password { get; init; }
}

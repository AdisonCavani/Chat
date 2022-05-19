namespace Chat.Core.Models.Requests;

public class LoginWith2faCredentialsDto : LoginCredentialsDto
{
    public string AuthenticatorCode { get; init; }

    public bool RememberMe { get; init; }
}

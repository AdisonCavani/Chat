namespace Chat.Core.Models.Requests;

public class PasswordRecoveryTokenDto : PasswordRecoveryDto
{
    public string Token { get; init; }
}

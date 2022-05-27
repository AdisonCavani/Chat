namespace Chat.Core.Models.Requests;

public class ResetPasswordDto
{
    public string Email { get; init; }

    public string Token { get; init; }

    public string NewPassword { get; init; }
}

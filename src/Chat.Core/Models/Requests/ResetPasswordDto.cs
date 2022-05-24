namespace Chat.Core.Models.Requests;

public class ResetPasswordDto
{
    public int UserId { get; init; }

    public string Token { get; init; }

    public string NewPassword { get; init; }
}

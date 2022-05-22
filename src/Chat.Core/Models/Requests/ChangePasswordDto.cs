namespace Chat.Core.Models.Requests;

public class ChangePasswordDto
{
    public string CurrentPassword { get; init; }

    public string NewPassword { get; init; }
}

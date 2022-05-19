namespace Chat.Core.Models.Requests;

public class ConfirmEmailDto
{
    public string UserId { get; init; }

    public string Token { get; init; }
}

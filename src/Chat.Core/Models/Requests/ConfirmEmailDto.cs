namespace Chat.Core.Models.Requests;

public class ConfirmEmailDto
{
    public int UserId { get; init; }

    public string Token { get; init; }
}

namespace Chat.Core.Models.Responses;

public class AuthSuccessResponse
{
    public string Token { get; init; }

    public string RefreshToken { get; init; }
}
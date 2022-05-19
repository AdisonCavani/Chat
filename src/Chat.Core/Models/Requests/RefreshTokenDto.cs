namespace Chat.Core.Models.Requests;

public class RefreshTokenDto
{
    public string Token { get; init; }
    
    public string RefreshToken { get; init; }
}
namespace Chat.Core.Models.Requests;

public class RegisterCredentialsDto
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string Password { get; init; }
}

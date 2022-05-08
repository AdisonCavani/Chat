using Chat.Core.DataModels;

namespace Chat.Core.ApiModels.UserProfile;

/// <summary>
/// The result of a login request or get user profile details request via API
/// </summary>
public class UserProfileDetailsDto
{
    /// <summary>
    /// The authentication token used to stay authenticated through future requests
    /// </summary>
    /// <remarks>The Token is only provided when called from the login methods</remarks>
    public string Token { get; set; }

    /// <summary>
    /// The users first name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The users last name
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The users username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The users email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Creates a new <see cref="LoginCredentialsDataModel"/>
    /// from this model
    /// </summary>
    /// <returns></returns>
    public LoginCredentialsDataModel ToLoginCredentialsDataModel()
    {
        return new LoginCredentialsDataModel
        {
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            Username = Username,
            Token = Token
        };
    }
}

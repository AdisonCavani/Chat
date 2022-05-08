namespace Chat.Core.ApiModels.UserProfile;

public class VerifyUserEmailDto
{
    public int UserId { get; set; }

    public string EmailToken { get; set; }
}

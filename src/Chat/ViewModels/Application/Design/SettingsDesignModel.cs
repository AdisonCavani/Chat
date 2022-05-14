namespace Chat.ViewModels.Application.Design;

public class SettingsDesignModel : SettingsViewModel
{
    public static SettingsDesignModel Instance => new();

    public SettingsDesignModel() : base(null, null, null)
    {
        FirstName = new() { Label = "Fist Name", OriginalText = "Luke" };
        LastName = new() { Label = "Last Name", OriginalText = "Malpass" };
        Username = new() { Label = "Username", OriginalText = "luke" };
        Password = new() { Label = "Password", FakePassword = "********" };
        Email = new() { Label = "Email", OriginalText = "contact@angelsix.com" };
    }
}

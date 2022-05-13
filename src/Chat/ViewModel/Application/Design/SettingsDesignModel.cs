using Chat.ViewModel.Input;

namespace Chat.ViewModel.Application.Design;

public class SettingsDesignModel : SettingsViewModel
{
    public static SettingsDesignModel Instance => new();

    public SettingsDesignModel()
    {
        FirstName = new TextEntryViewModel { Label = "Fist Name", OriginalText = "Luke" };
        LastName = new TextEntryViewModel { Label = "Last Name", OriginalText = "Malpass" };
        Username = new TextEntryViewModel { Label = "Username", OriginalText = "luke" };
        Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
        Email = new TextEntryViewModel { Label = "Email", OriginalText = "contact@angelsix.com" };
    }
}

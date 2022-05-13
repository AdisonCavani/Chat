namespace Chat.ViewModel.Input.Design;

public class PasswordEntryDesignModel : PasswordEntryViewModel
{
    public static PasswordEntryDesignModel Instance => new();

    public PasswordEntryDesignModel()
    {
        Label = "Name";
        FakePassword = "********";
    }
}

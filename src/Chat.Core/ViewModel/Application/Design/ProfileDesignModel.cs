namespace Chat.Core;

/// <summary>
/// The design-time data for a <see cref="ProfileViewModel"/>
/// </summary>
public class ProfileDesignModel : ProfileViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static ProfileDesignModel Instance => new();

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProfileDesignModel()
    {
        FirstName = new TextEntryViewModel { Label = "First name", TagText = "First name", OriginalText = "Adison" };
        LastName = new TextEntryViewModel { Label = "Last name", TagText = "Last name", OriginalText = null };
        Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
        Email = new TextEntryViewModel { Label = "Email", TagText = "Email", OriginalText = "test@email.com" };
    }

    #endregion
}

namespace Chat.Core
{
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
            Name = new TextEntryViewModel { Label = "Name", OriginalText = "Adison Cavani" };
            Username = new TextEntryViewModel { Label = "Username", OriginalText = "adison" };
            Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "fake-design" };
            Email = new TextEntryViewModel { Label = "Email", OriginalText = "test@email.com" };
        }

        #endregion
    }
}

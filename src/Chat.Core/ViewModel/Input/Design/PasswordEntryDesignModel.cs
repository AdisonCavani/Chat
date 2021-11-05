namespace Chat.Core
{
    /// <summary>
    /// The design-time data for a <see cref="PasswordEntryDesignModel"/>
    /// </summary>
    public class PasswordEntryDesignModel : PasswordEntryViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static PasswordEntryDesignModel Instance => new();

        #endregion

        #region Constructor

        public PasswordEntryDesignModel()
        {
            Label = "Name";
            FakePassword = "********";
        }

        #endregion
    }
}

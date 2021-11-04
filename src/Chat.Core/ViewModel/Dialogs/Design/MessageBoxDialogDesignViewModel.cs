namespace Chat.Core
{
    /// <summary>
    /// The design-time data for a <see cref="MessageBoxDialogViewModel"/>
    /// </summary>
    public class MessageBoxDialogDesignViewModel : MessageBoxDialogViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MessageBoxDialogDesignViewModel Instance => new();

        #endregion

        #region Constructor

        public MessageBoxDialogDesignViewModel()
        {
            OkText = "OK";
            Message = "Design time message";
        }

        #endregion
    }
}

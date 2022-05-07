namespace Chat.ViewModel.PopupMenu.Design;

/// <summary>
/// The design-time data for a <see cref="ChatAttachmentPopupMenuViewModel"/>
/// </summary>
public class ChatAttachmentPopupMenuDesignModel : ChatAttachmentPopupMenuViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static ChatAttachmentPopupMenuDesignModel Instance => new();

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public ChatAttachmentPopupMenuDesignModel()
    {
    }

    #endregion
}

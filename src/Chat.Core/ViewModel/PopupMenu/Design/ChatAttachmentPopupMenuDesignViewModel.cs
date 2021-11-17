namespace Chat.Core;

/// <summary>
/// A design-time data for a <see cref="ChatAttachmentPopupMenuViewModel"/>
/// </summary>
public class ChatAttachmentPopupMenuDesignViewModel : ChatAttachmentPopupMenuViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static ChatAttachmentPopupMenuDesignViewModel Instance => new();

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public ChatAttachmentPopupMenuDesignViewModel()
    {

    }

    #endregion
}

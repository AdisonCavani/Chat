using System.Collections.Generic;

namespace Chat;

/// <summary>
/// A view model for each chat list item in the overview chat list
/// </summary>
public class ChatListViewModel : BaseViewModel
{
    /// <summary>
    /// The chat list items for the list
    /// </summary>
    public List<ChatListItemViewModel> Items { get; set; }
}

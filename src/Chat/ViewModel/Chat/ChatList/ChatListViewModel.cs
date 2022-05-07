using System.Collections.Generic;
using Chat.ViewModel.Base;

namespace Chat.ViewModel.Chat.ChatList;

/// <summary>
/// A view model for the overview chat list
/// </summary>
public class ChatListViewModel : BaseViewModel
{
    /// <summary>
    /// The chat list items for the list
    /// </summary>
    public List<ChatListItemViewModel> Items { get; set; }
}

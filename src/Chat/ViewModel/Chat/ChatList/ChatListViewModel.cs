using System.Collections.Generic;

namespace Chat.ViewModel.Chat.ChatList;

/// <summary>
/// A view model for the overview chat list
/// </summary>
public class ChatListViewModel
{
    public List<ChatListItemViewModel> Items { get; set; }
}

using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModels.Chat.ChatList;

/// <summary>
/// A view model for the overview chat list
/// </summary>
public partial class ChatListViewModel : ObservableObject
{
    [ObservableProperty]
    private List<ChatListItemViewModel>? items;
}

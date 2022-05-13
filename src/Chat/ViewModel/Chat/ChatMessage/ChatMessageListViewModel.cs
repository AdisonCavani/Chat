using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Chat.ViewModel.PopupMenu;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chat.ViewModel.Chat.ChatMessage;

/// <summary>
/// A view model for a chat message thread list
/// </summary>
public partial class ChatMessageListViewModel : ObservableObject
{
    protected string? lastSearchText;

    [ObservableProperty]
    private ObservableCollection<ChatMessageListItemViewModel>? filteredItems;

    [ObservableProperty]
    private string? displayTitle;

    [ObservableProperty]
    private bool attachmentMenuVisible;

    [ObservableProperty]
    private ChatAttachmentPopupMenuViewModel? attachmentMenu;

    [ObservableProperty]
    private string? pendingMessageText;

    protected ObservableCollection<ChatMessageListItemViewModel>? items;

    /// <summary>
    /// The chat thread items for the list
    /// NOTE: Do not call Items.Add to add messages to this list
    ///       as it will make the FilteredItems out of sync
    /// </summary>
    public ObservableCollection<ChatMessageListItemViewModel>? Items
    {
        get => items;
        set
        {
            if (SetProperty(ref items, value)) // Update filtered list to match
                FilteredItems = new ObservableCollection<ChatMessageListItemViewModel>(items);
        }
    }

    protected string? searchText;

    public string? SearchText
    {
        get => searchText;
        set
        {
            if (SetProperty(ref searchText, value) && string.IsNullOrWhiteSpace(SearchText))
                Search(); // Search to restore messages
        }
    }

    protected bool searchIsOpen;

    public bool SearchIsOpen
    {
        get => searchIsOpen;
        set
        {
            if (SetProperty(ref searchIsOpen, value) && !searchIsOpen)
                SearchText = string.Empty;
        }
    }

    public bool AnyPopupVisible => AttachmentMenuVisible;

    public ChatMessageListViewModel()
    {
        // Make a default menu
        AttachmentMenu = new ChatAttachmentPopupMenuViewModel();
    }

    [ICommand]
    private void AttachmentButton()
    {
        AttachmentMenuVisible ^= true;
    }

    [ICommand]
    private void PopupClickaway()
    {
        AttachmentMenuVisible = false;
    }

    [ICommand]
    internal void Send()
    {
        if (string.IsNullOrWhiteSpace(PendingMessageText))
            return;

        if (Items is null)
            Items = new();

        if (FilteredItems is null)
            FilteredItems = new();

        // Fake send a new message
        ChatMessageListItemViewModel message = new()
        {
            Initials = "LM",
            Message = PendingMessageText,
            MessageSentTime = DateTime.UtcNow,
            SentByMe = true,
            SenderName = "Luke Malpass",
            NewItem = true
        };

        // Add message to both lists
        Items.Add(message);
        FilteredItems.Add(message);

        // Clear the pending message text
        PendingMessageText = string.Empty;
    }

    [ICommand]
    private void Search()
    {
        // Make sure we don't re-search the same text
        if ((string.IsNullOrEmpty(lastSearchText) && string.IsNullOrEmpty(SearchText)) ||
            string.Equals(lastSearchText, SearchText))
            return;

        // If we have no search text, or no items
        if (string.IsNullOrWhiteSpace(SearchText) || Items is null || Items.Count <= 0)
        {
            // Make filtered list the same
            FilteredItems = new ObservableCollection<ChatMessageListItemViewModel>(Items ?? Enumerable.Empty<ChatMessageListItemViewModel>());

            // Set last search text
            lastSearchText = SearchText;

            return;
        }

        // Find all items that contain the given text
        // TODO: Make more efficient search
        FilteredItems = new ObservableCollection<ChatMessageListItemViewModel>(
            Items.Where(item => item.Message.ToLower().Contains(SearchText)));

        // Set last search text
        lastSearchText = SearchText;
    }

    [ICommand]
    private void ClearSearch()
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
            SearchText = string.Empty;
        else
            SearchIsOpen = false;
    }

    [ICommand]
    private void OpenSearch() => SearchIsOpen = true;

    [ICommand]
    private void CloseSearch() => SearchIsOpen = false;
}

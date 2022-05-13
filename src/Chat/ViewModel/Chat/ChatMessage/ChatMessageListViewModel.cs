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
    protected string mLastSearchText;

    protected string mSearchText;

    protected ObservableCollection<ChatMessageListItemViewModel> mItems;

    protected bool mSearchIsOpen;

    /// <summary>
    /// The chat thread items for the list
    /// NOTE: Do not call Items.Add to add messages to this list
    ///       as it will make the FilteredItems out of sync
    /// </summary>
    public ObservableCollection<ChatMessageListItemViewModel> Items
    {
        get => mItems;
        set
        {
            // Make sure list has changed
            if (mItems == value)
                return;

            // Update value
            mItems = value;

            // Update filtered list to match
            FilteredItems = new ObservableCollection<ChatMessageListItemViewModel>(mItems);
        }
    }

    public ObservableCollection<ChatMessageListItemViewModel> FilteredItems { get; set; }

    public string DisplayTitle { get; set; }

    public bool AttachmentMenuVisible { get; set; }

    public bool AnyPopupVisible => AttachmentMenuVisible;

    public ChatAttachmentPopupMenuViewModel AttachmentMenu { get; set; }

    public string PendingMessageText { get; set; }

    public string SearchText
    {
        get => mSearchText;
        set
        {
            if (!SetProperty(ref mSearchText, value))
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
                Search(); // Search to restore messages
        }
    }

    public bool SearchIsOpen
    {
        get => mSearchIsOpen;
        set
        {
            if (!SetProperty(ref mSearchIsOpen, value))
                return;

            if (!mSearchIsOpen)
                SearchText = string.Empty;
        }
    }

    public ChatMessageListViewModel()
    {
        // Make a default menu
        AttachmentMenu = new ChatAttachmentPopupMenuViewModel();
    }

    [ICommand]
    public void AttachmentButton()
    {
        AttachmentMenuVisible ^= true;
    }

    [ICommand]
    public void PopupClickaway()
    {
        AttachmentMenuVisible = false;
    }

    [ICommand]
    public void Send()
    {
        // Don't send a blank message
        if (string.IsNullOrWhiteSpace(PendingMessageText))
            return;

        // Ensure lists are not null
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
    public void Search()
    {
        // Make sure we don't re-search the same text
        if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
            string.Equals(mLastSearchText, SearchText))
            return;

        // If we have no search text, or no items
        if (string.IsNullOrWhiteSpace(SearchText) || Items is null || Items.Count <= 0)
        {
            // Make filtered list the same
            FilteredItems = new ObservableCollection<ChatMessageListItemViewModel>(Items ?? Enumerable.Empty<ChatMessageListItemViewModel>());

            // Set last search text
            mLastSearchText = SearchText;

            return;
        }

        // Find all items that contain the given text
        // TODO: Make more efficient search
        FilteredItems = new ObservableCollection<ChatMessageListItemViewModel>(
            Items.Where(item => item.Message.ToLower().Contains(SearchText)));

        // Set last search text
        mLastSearchText = SearchText;
    }

    [ICommand]
    public void ClearSearch()
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
            SearchText = string.Empty;
        else
            SearchIsOpen = false;
    }

    [ICommand]
    public void OpenSearch() => SearchIsOpen = true;

    [ICommand]
    public void CloseSearch() => SearchIsOpen = false;
}

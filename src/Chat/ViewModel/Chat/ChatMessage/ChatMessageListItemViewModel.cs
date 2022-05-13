using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.Chat.ChatMessage;

/// <summary>
/// A view model for each chat message thread item in a chat thread
/// </summary>
public class ChatMessageListItemViewModel : ObservableObject
{
    public string SenderName { get; set; }

    public string Message { get; set; }

    public string Initials { get; set; }

    public string ProfilePictureRGB { get; set; }

    public bool IsSelected { get; set; }

    public bool SentByMe { get; set; }

    /// <summary>
    /// The time the message was read, or <see cref="DateTimeOffset.MinValue"/> if not read
    /// </summary>
    public DateTimeOffset MessageReadTime { get; set; }

    public bool MessageRead => MessageReadTime > DateTimeOffset.MinValue;

    public DateTimeOffset MessageSentTime { get; set; }

    public bool NewItem { get; set; }

    public ChatMessageListItemImageAttachmentViewModel ImageAttachment { get; set; }

    public bool HasMessage => Message is not null;

    public bool HasImageAttachment => ImageAttachment is not null;
}

using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModels.Chat.ChatMessage;

/// <summary>
/// A view model for each chat message thread item in a chat thread
/// </summary>
public partial class ChatMessageListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string? senderName;

    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private string? initials;

    [ObservableProperty]
    private string? profilePictureRGB;

    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private bool sentByMe;

    /// <summary>
    /// The time the message was read, or <see cref="DateTimeOffset.MinValue"/> if not read
    /// </summary>
    [ObservableProperty]
    private DateTimeOffset messageReadTime;

    [ObservableProperty]
    private DateTimeOffset messageSentTime;

    [ObservableProperty]
    private bool newItem;

    [ObservableProperty]
    private ChatMessageListItemImageAttachmentViewModel? imageAttachment;

    public bool MessageRead => MessageReadTime > DateTimeOffset.MinValue;

    public bool HasMessage => Message is not null;

    public bool HasImageAttachment => ImageAttachment is not null;
}

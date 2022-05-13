using System;

namespace Chat.ViewModels.Chat.ChatMessage.Design;

/// <summary>
/// The design-time data for a <see cref="ChatMessageListItemViewModel"/>
/// </summary>
public class ChatMessageListItemDesignModel : ChatMessageListItemViewModel
{
    public static ChatMessageListItemDesignModel Instance => new();

    public ChatMessageListItemDesignModel()
    {
        Initials = "LM";
        SenderName = "Luke";
        Message = "Some design time visual text";
        ProfilePictureRGB = "3099c5";
        SentByMe = true;
        MessageSentTime = DateTimeOffset.UtcNow;
        MessageReadTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(1.3));
    }
}

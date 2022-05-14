namespace Chat.ViewModels.Chat.ChatList.Design;

public class ChatListItemDesignModel : ChatListItemViewModel
{
    public static ChatListItemDesignModel Instance => new();

    public ChatListItemDesignModel() : base(null)
    {
        Initials = "LM";
        Name = "Luke";
        Message = "This chat app is awesome! I bet it will be fast too";
        ProfilePictureRGB = "3099c5";
    }
}

namespace Chat.ViewModel.Chat.ChatList.Design;

public class ChatListItemDesignModel : ChatListItemViewModel
{
    public static ChatListItemDesignModel Instance => new();

    public ChatListItemDesignModel()
    {
        Initials = "LM";
        Name = "Luke";
        Message = "This chat app is awesome! I bet it will be fast too";
        ProfilePictureRGB = "3099c5";
    }
}

namespace Chat.ViewModels.Controls.Design;

public class ChatMessageItemDesignViewModel : ChatMessageItemViewModel
{
    public ChatMessageItemDesignViewModel()
    {
        Message = "Witam, co tam? U mnie wszystko dobrze...";
        Send = new(2022, 06, 18, 19, 08, 02);
    }
}

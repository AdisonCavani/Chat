namespace Chat.ViewModel.Dialogs.Design;

public class MessageBoxDialogDesignModel : MessageBoxDialogViewModel
{
    public static MessageBoxDialogDesignModel Instance => new();

    public MessageBoxDialogDesignModel()
    {
        OkText = "OK";
        Message = "Design time messages are fun :)";
    }
}

namespace Chat.ViewModel.Dialogs;

public class MessageBoxDialogViewModel : BaseDialogViewModel
{
    public string Message { get; set; }

    public string OkText { get; set; } = "OK";
}

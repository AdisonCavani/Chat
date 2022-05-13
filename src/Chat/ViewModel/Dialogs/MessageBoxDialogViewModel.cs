using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.Dialogs;

public partial class MessageBoxDialogViewModel : BaseDialogViewModel
{
    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private string? okText = "OK";
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModels.Dialogs;

public partial class BaseDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string? title;
}

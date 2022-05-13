using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.Dialogs;

public partial class BaseDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string? title;
}

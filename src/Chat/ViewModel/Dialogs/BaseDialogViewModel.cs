using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.Dialogs;

public class BaseDialogViewModel : ObservableObject
{
    public string Title { get; set; }
}

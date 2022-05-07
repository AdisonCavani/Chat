using Chat.ViewModel.Base;

namespace Chat.ViewModel.Dialogs;

/// <summary>
/// A base view model for any dialogs
/// </summary>
public class BaseDialogViewModel : BaseViewModel
{
    /// <summary>
    /// The title of the dialog
    /// </summary>
    public string Title { get; set; }
}

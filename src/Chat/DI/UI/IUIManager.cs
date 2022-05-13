using System.Threading.Tasks;
using MessageBoxDialogViewModel = Chat.ViewModels.Dialogs.MessageBoxDialogViewModel;

namespace Chat.DI.UI;

/// <summary>
/// The UI manager that handles any UI interaction in the application
/// </summary>
public interface IUIManager
{
    /// <summary>
    /// Displays a single message box to the user
    /// </summary>
    /// <param name="viewModel">The view model</param>
    /// <returns></returns>
    Task ShowMessage(MessageBoxDialogViewModel viewModel);
}

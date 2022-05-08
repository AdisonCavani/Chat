using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;

namespace Chat.WPFViewModels;

/// <summary>
/// The View Model for the custom flat window
/// </summary>
public partial class DialogWindowViewModel : WindowViewModel
{
    /// <summary>
    /// The title of this dialog window
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The content to host inside the dialog
    /// </summary>
    public Control Content { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public DialogWindowViewModel() : base()
    {
        // Make minimum size smaller
        WindowMinimumWidth = 250;
        WindowMinimumHeight = 100;

        // Make title bar smaller
        TitleHeight = 30;
    }

    [ICommand]
    private void Close(object parameter)
    {
        ((Window)parameter).Close();
    }
}

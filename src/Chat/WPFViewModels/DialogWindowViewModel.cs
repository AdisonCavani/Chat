using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;

namespace Chat.WPFViewModels;

/// <summary>
/// The View Model for the custom flat window
/// </summary>
public partial class DialogWindowViewModel : WindowViewModel
{
    public string Title { get; set; }

    public Control Content { get; set; }

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

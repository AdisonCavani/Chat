using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chat.ViewModels.Wpf;

/// <summary>
/// The View Model for the custom flat window
/// </summary>
public partial class DialogWindowViewModel : WindowViewModel
{
    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private Control? content;

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

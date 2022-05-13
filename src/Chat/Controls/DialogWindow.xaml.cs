using Chat.ViewModels.Wpf;

namespace Chat.Controls;

/// <summary>
/// Interaction logic for DialogWindow.xaml
/// </summary>
public partial class DialogWindow
{
    /// <summary>
    /// The view model for this window
    /// </summary>
    private DialogWindowViewModel mViewModel;

    /// <summary>
    /// The view model for this window
    /// </summary>
    public DialogWindowViewModel ViewModel
    {
        get => mViewModel;
        set
        {
            // Set new value
            mViewModel = value;

            // Update data context
            DataContext = mViewModel;
        }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public DialogWindow()
    {
        InitializeComponent();
    }
}

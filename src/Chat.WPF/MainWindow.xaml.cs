using System.Windows;

namespace Chat.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        
    }

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}

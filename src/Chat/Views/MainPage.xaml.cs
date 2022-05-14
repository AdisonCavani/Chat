using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        DataContext = App.Current.Services.GetService<SignalrViewModel>();

        InitializeComponent();
    }
}

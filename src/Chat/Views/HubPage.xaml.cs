using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views;

public sealed partial class HubPage : Page
{
    public HubPage()
    {
        DataContext = App.Current.Services.GetRequiredService<SignalrViewModel>();

        InitializeComponent();
    }
}

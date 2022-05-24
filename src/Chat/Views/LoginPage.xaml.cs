using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views;

public sealed partial class LoginPage : Page
{
    public LoginPage()
    {
        DataContext = App.Current.Services.GetService<LoginViewModel>();

        InitializeComponent();
    }
}

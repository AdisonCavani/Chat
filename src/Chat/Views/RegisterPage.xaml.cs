using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views;

public sealed partial class RegisterPage : Page
{
    public RegisterPage()
    {
        DataContext = App.Current.Services.GetService<RegisterViewModel>();

        InitializeComponent();
    }
}

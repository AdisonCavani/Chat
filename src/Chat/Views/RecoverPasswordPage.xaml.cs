using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views;

public sealed partial class RecoverPasswordPage : Page
{
    public RecoverPasswordPage()
    {
        DataContext = App.Current.Services.GetRequiredService<RecoverPasswordViewModel>();

        InitializeComponent();
    }
}

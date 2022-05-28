using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views.Password;

public sealed partial class TokenPage : Page
{
    public TokenPage()
    {
        DataContext = App.Current.Services.GetRequiredService<RecoverPasswordViewModel>();

        InitializeComponent();
    }
}

using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views.Password;

public sealed partial class RecoverPage : Page
{
    public RecoverPage()
    {
        DataContext = App.Current.Services.GetRequiredService<RecoverPasswordViewModel>();

        InitializeComponent();
    }
}

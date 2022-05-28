using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views.Password;

public sealed partial class ChangePage : Page
{
    public ChangePage()
    {
        DataContext = App.Current.Services.GetRequiredService<RecoverPasswordViewModel>();

        InitializeComponent();
    }
}

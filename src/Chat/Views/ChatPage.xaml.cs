using Chat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Views;

public sealed partial class ChatPage : Page
{
    public ChatPage()
    {
        DataContext = App.Current.Services.GetRequiredService<ChatViewModel>();

        InitializeComponent();
    }
}

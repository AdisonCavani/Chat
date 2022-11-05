using Chat.ViewModels.Controls.Design;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Controls;

public sealed partial class ChatListBar : UserControl
{
    public ChatListBar()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetRequiredService<UserListDesignViewModel>();
    }
}

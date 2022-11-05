using Chat.ViewModels.Controls.Design;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.Controls;

public sealed partial class ChatList : UserControl
{
    public ChatList()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetRequiredService<UserListDesignViewModel>();
    }
}

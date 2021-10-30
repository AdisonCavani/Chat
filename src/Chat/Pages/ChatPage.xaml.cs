using System.Windows.Markup;
using Chat.Core;

namespace Chat
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : BasePage<ChatMessageListViewModel>, IComponentConnector
    {
        public ChatPage()
        {
            InitializeComponent();
        }
    }
}

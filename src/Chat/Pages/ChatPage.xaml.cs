using System.Windows.Markup;
using Chat.Core;

namespace Chat
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class ChatPage : BasePage<LoginViewModel>, IComponentConnector
    {
        public ChatPage()
        {
            InitializeComponent();
        }
    }
}

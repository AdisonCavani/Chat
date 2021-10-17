using System.Security;
using System.Windows.Markup;

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

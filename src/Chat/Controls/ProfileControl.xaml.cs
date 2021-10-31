using System.Windows.Controls;
using Chat.Core;

namespace Chat
{
    /// <summary>
    /// Interaction logic for ProfileControl.xaml
    /// </summary>
    public partial class ProfileControl : UserControl
    {
        public ProfileControl()
        {
            InitializeComponent();

            // Set data context to profile view model
            DataContext = IoC.Profile;
        }
    }
}

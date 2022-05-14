using System.Security;
using Chat.ViewModels.Application;

namespace Chat.Views;

/// <summary>
/// Interaction logic for LoginPage.xaml
/// </summary>
public partial class LoginPage
{
    /// <summary>
    /// Constructor with specific view model
    /// </summary>
    public LoginPage()
    {
        InitializeComponent();
    }

    public LoginPage(LoginViewModel specificViewModel) : base(specificViewModel)
    {
        InitializeComponent();
    }

    /// <summary>
    /// The secure password for this login page
    /// </summary>
    public SecureString SecurePassword => PasswordText.SecurePassword;
}

using System.Security;
using Chat.ViewModels.Base;
using LoginViewModel = Chat.ViewModels.Application.LoginViewModel;

namespace Chat.Views;

/// <summary>
/// Interaction logic for LoginPage.xaml
/// </summary>
public partial class LoginPage : IHavePassword
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public LoginPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Constructor with specific view model
    /// </summary>
    public LoginPage(LoginViewModel specificViewModel) : base(specificViewModel)
    {
        InitializeComponent();
    }

    /// <summary>
    /// The secure password for this login page
    /// </summary>
    public SecureString SecurePassword => PasswordText.SecurePassword;
}

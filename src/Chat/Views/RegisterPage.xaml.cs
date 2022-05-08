using System.Security;
using Chat.ViewModel.Application;
using Chat.ViewModel.Base;

namespace Chat.Views;

/// <summary>
/// Interaction logic for RegisterPage.xaml
/// </summary>
public partial class RegisterPage : IHavePassword
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public RegisterPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Constructor with specific view model
    /// </summary>
    public RegisterPage(RegisterViewModel specificViewModel) : base(specificViewModel)
    {
        InitializeComponent();
    }

    /// <summary>
    /// The secure password for this login page
    /// </summary>
    public SecureString SecurePassword => PasswordText.SecurePassword;
}

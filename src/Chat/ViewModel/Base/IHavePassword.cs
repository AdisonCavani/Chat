using System.Security;

namespace Chat.ViewModel.Base;

public interface IHavePassword
{
    SecureString SecurePassword { get; }
}

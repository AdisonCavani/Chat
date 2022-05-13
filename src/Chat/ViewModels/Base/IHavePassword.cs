using System.Security;

namespace Chat.ViewModels.Base;

public interface IHavePassword
{
    SecureString SecurePassword { get; }
}

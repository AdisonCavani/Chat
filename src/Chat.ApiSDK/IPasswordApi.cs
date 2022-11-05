using Chat.Core;
using Chat.Core.Models.Requests;
using Refit;
using System.Threading.Tasks;

namespace Chat.ApiSDK;

public interface IPasswordApi
{
    [Get(ApiRoutes.Account.Password.SendRecoveryEmail)]
    Task SendRecoveryEmailAsync([Query] PasswordRecoveryDto dto);

    [Post(ApiRoutes.Account.Password.VerifyToken)]
    Task VerifyTokenAsync([Body] PasswordRecoveryTokenDto dto);

    [Post(ApiRoutes.Account.Password.Reset)]
    Task ResetAsync([Body] ResetPasswordDto dto);
    // Task ResetAsync([Body] ResetPasswordDto dto, CancellationToken token);

    [Post(ApiRoutes.Account.Password.Change)]
    Task ChangePasswordAsync(ChangePasswordDto dto, [Authorize] string jwtToken);
    // Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken token, [Authorize] string jwtToken);
}

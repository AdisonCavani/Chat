using Chat.Core;
using Chat.Core.Models.Requests;
using Chat.Core.Models.Responses;
using Refit;
using System.Threading.Tasks;

namespace Chat.ApiSDK;

public interface IAccountApi
{
    [Post(ApiRoutes.Account.Register)]
    Task RegisterAsync([Body] RegisterCredentialsDto dto);
    // Task RegisterAsync([Body] RegisterCredentialsDto dto, CancellationToken token);

    [Get(ApiRoutes.Account.ConfirmEmail)]
    Task ConfirmEmailAsync([Query] ConfirmEmailDto dto);

    [Post(ApiRoutes.Account.Login)]
    Task<JwtTokenDto> LoginAsync([Body] LoginCredentialsDto dto);

    [Get(ApiRoutes.Account.ResendVerificationEmail)]
    Task ResendVerificationEmailAsync([Query] ResendVerificationEmailDto dto);
    // Task ResendVerificationEmailAsync([Query] ResendVerificationEmailDto dto, CancellationToken token);
}

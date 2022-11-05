using Chat.Core;
using Chat.Core.Models.Entities;
using Refit;
using System.Threading.Tasks;

namespace Chat.ApiSDK;

public interface IProfileApi
{
    [Get(ApiRoutes.Account.Profile.Details)]
    Task<UserProfile> GetUserDetailsAsync([Authorize] string jwtToken);
}

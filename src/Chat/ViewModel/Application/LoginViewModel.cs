using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Core.ApiModels;
using Chat.Core.ApiModels.LoginRegister;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.DataModels;
using Chat.Core.Routes;
using Chat.Core.Security;
using Chat.ViewModel.Base;
using Chat.WebRequests;
using CommunityToolkit.Mvvm.Input;
using static Chat.DI.DI;

namespace Chat.ViewModel.Application;

public partial class LoginViewModel : BaseViewModel
{
    public string? Email { get; set; }

    public bool LoginIsRunning { get; set; }

    [ICommand]
    private async Task Login(object parameter)
    {
        await RunCommandAsync(() => LoginIsRunning, async () =>
        {
            // Call the server and attempt to login with credentials
            var result = await Dna.WebRequests.PostAsync<ApiResponse<UserProfileDetailsDto>>(
                RouteHelpers.GetAbsoluteRoute(ApiRoutes.Login),
                new LoginCredentialsDto
                {
                    UsernameOrEmail = Email,
                    Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                });

            if (await result.HandleErrorIfFailedAsync("Login Failed") || result.ServerResponse.Response is null)
                return;

            await ApplicationViewModel.HandleSuccessfulLoginAsync(result.ServerResponse.Response);
        });
    }

    [ICommand]
    private async Task Register()
    {
        ViewModelApplication.GoToPage(ApplicationPage.Register);
        await Task.Delay(1);
    }
}

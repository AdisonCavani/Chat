using System.Threading.Tasks;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Chat.DI.DI;

namespace Chat.ViewModels.Application;

public partial class ApplicationViewModel : ObservableObject
{
    private bool settingsMenuVisible;

    public bool SettingsMenuVisible
    {
        get => settingsMenuVisible;
        set
        {
            if (SetProperty(ref settingsMenuVisible, value) && value)
                Task.Run(ViewModelSettings.Load); // Reload settings
        }
    }

    /// <summary>
    /// The view model to use for the current page when the CurrentPage changes
    /// NOTE: This is not a live up-to-date view model of the current page
    ///       it is simply used to set the view model of the current page 
    ///       at the time it changes
    /// </summary>
    [ObservableProperty]
    private ObservableObject? currentPageViewModel;

    [ObservableProperty]
    private ApplicationPage currentPage = ApplicationPage.Login;

    [ObservableProperty]
    private bool sideMenuVisible;

    [ObservableProperty]
    private SideMenuContent currentSideMenuContent = SideMenuContent.Chat;

    [ObservableProperty]
    private bool serverReachable = true;

    [ICommand]
    public void OpenChat()
    {
        CurrentSideMenuContent = SideMenuContent.Chat;
    }

    [ICommand]
    public void OpenContacts()
    {
        CurrentSideMenuContent = SideMenuContent.Contacts;
    }

    [ICommand]
    public void OpenMedia()
    {
        CurrentSideMenuContent = SideMenuContent.Media;
    }

    public void GoToPage(ApplicationPage page, ObservableObject? viewModel = null)
    {
        SettingsMenuVisible = false;

        CurrentPageViewModel = viewModel;

        var different = CurrentPage != page;

        CurrentPage = page;

        // If the page hasn't changed, fire off notification
        // So pages still update if just the view model has changed
        if (!different)
            OnPropertyChanged(nameof(CurrentPage));

        SideMenuVisible = page == ApplicationPage.Chat;
    }

    public static async Task HandleSuccessfulLoginAsync(UserProfileDetailsDto loginResult)
    {
        await ClientDataStore.SaveLoginCredentialsAsync(loginResult);
        await ViewModelSettings.Load(); // Load new settings
        ViewModelApplication.GoToPage(ApplicationPage.Chat);
    }
}

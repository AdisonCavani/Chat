using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Chat.DI.DI;

namespace Chat.ViewModel.Application;

public partial class ApplicationViewModel : ObservableObject
{
    private bool mSettingsMenuVisible;

    public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Login;

    /// <summary>
    /// The view model to use for the current page when the CurrentPage changes
    /// NOTE: This is not a live up-to-date view model of the current page
    ///       it is simply used to set the view model of the current page 
    ///       at the time it changes
    /// </summary>
    public ObservableObject? CurrentPageViewModel { get; set; }

    public bool SideMenuVisible { get; set; }

    public bool SettingsMenuVisible
    {
        get => mSettingsMenuVisible;
        set
        {
            if (mSettingsMenuVisible == value)
                return;

            // Set the backing field
            mSettingsMenuVisible = value;

            if (value)
                Task.Run(ViewModelSettings.Load); // Reload settings
        }
    }

    public SideMenuContent CurrentSideMenuContent { get; set; } = SideMenuContent.Chat;

    public bool ServerReachable { get; set; } = true;

    [ICommand]
    public void OpenChat()
    {
        // Set the current side menu to Chat
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
        // Always hide settings page if we are changing pages
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
        await ClientDataStore.SaveLoginCredentialsAsync(loginResult.ToLoginCredentialsDataModel());
        await ViewModelSettings.Load(); // Load new settings
        ViewModelApplication.GoToPage(ApplicationPage.Chat);
    }
}

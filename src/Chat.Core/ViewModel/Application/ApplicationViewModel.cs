﻿using System.Threading.Tasks;

namespace Chat.Core;

/// <summary>
/// The application state as a view model
/// </summary>
public class ApplicationViewModel : BaseViewModel
{
    /// <summary>
    /// The current page of the application
    /// </summary>
    public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Chat;

    /// <summary>
    /// The view model to use for the current page when the CurrentPage changes
    /// NOTE: This is not a live up-to-date view model of the current page
    ///       it is simply used to set the view model of the current page
    ///       at the time it changes
    /// </summary>
    public BaseViewModel CurrentPageViewModel { get; private set; }

    /// <summary>
    /// True if the side menu should be shown
    /// </summary>
    public bool SideMenuVisible { get; set; }

    /// <summary>
    /// True if the profile menu should be shown
    /// </summary>
    public bool ProfileMenuVisible { get; set; }

    /// <summary>
    /// Navigates to the specified page
    /// </summary>
    /// <param name="page">The page to go to</param>
    /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
    public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
    {
        // Always hide settings page if we are changing pages
        ProfileMenuVisible = false;

        // Set the view model
        CurrentPageViewModel = viewModel;

        // See if page has changed
        var different = CurrentPage != page;

        // Set the current page
        CurrentPage = page;

        // If the page hasn't changed, fire off notification
        // So pages still update if just the view model has changed
        if (!different)
            OnPropertyChanged(nameof(CurrentPage));

        // Show side menu or not?
        SideMenuVisible = page == ApplicationPage.Chat;
    }

    /// <summary>
    /// Handles what happens when we have successfully logged in
    /// </summary>
    /// <param name="loginResult">The results from the successful login</param>
    /// <returns></returns>
    public async Task HandleSuccessfulLoginAsync(LoginResultApiModel loginResult)
    {
        // Store this in the client data store
        await IoC.ClientDataStore.SaveLoginCredentialsAsync(new LoginCredentialsDataModel
        {
            Email = loginResult.Email,
            Username = loginResult.Email,
            FirstName = loginResult.FirstName,
            LastName = loginResult.LastName,
            Token = loginResult.Token
        });

        // Load new settings
        await IoC.Profile.LoadAsync();

        // Go to chat page
        IoC.Application.GoToPage(ApplicationPage.Chat);
    }
}

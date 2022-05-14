﻿using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Chat.Animation;
using CommunityToolkit.Mvvm.ComponentModel;
using Dna;

namespace Chat.Views;

/// <summary>
/// The base page for all pages to gain base functionality
/// </summary>
public class BasePage : UserControl
{
    /// <summary>
    /// The View Model associated with this page
    /// </summary>
    private ObservableObject mViewModel;

    /// <summary>
    /// The animation the play when the page is first loaded
    /// </summary>
    public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

    /// <summary>
    /// The animation the play when the page is unloaded
    /// </summary>
    public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

    /// <summary>
    /// The time any slide animation takes to complete
    /// </summary>
    public float SlideSeconds { get; set; } = 0.4f;

    /// <summary>
    /// A flag to indicate if this page should animate out on load.
    /// Useful for when we are moving the page to another frame
    /// </summary>
    public bool ShouldAnimateOut { get; set; }

    /// <summary>
    /// The View Model associated with this page
    /// </summary>
    public ObservableObject ViewModelObject
    {
        get => mViewModel;
        set
        {
            // If nothing has changed, return
            if (mViewModel == value)
                return;

            // Update the value
            mViewModel = value;

            // Fire the view model changed method
            OnViewModelChanged();

            // Set the data context for this page
            DataContext = mViewModel;
        }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public BasePage()
    {
        // Don't bother animating in design time
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        // If we are animating in, hide to begin with
        if (PageLoadAnimation != PageAnimation.None)
            Visibility = Visibility.Collapsed;

        // Listen out for the page loading
        Loaded += BasePage_LoadedAsync;
    }

    /// <summary>
    /// Once the page is loaded, perform any required animation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void BasePage_LoadedAsync(object sender, RoutedEventArgs e)
    {
        // If we are setup to animate out on load
        if (ShouldAnimateOut)
            // Animate out the page
            await AnimateOutAsync();
        // Otherwise...
        else
            // Animate the page in
            await AnimateInAsync();
    }

    /// <summary>
    /// Animates the page in
    /// </summary>
    /// <returns></returns>
    public async Task AnimateInAsync()
    {
        // Make sure we have something to do
        if (PageLoadAnimation == PageAnimation.None)
            return;

        switch (PageLoadAnimation)
        {
            case PageAnimation.SlideAndFadeInFromRight:

                // Start the animation
                await this.SlideAndFadeInAsync(AnimationSlideInDirection.Right, false, SlideSeconds, size: (int)Application.Current.MainWindow.Width);

                break;
        }
    }

    /// <summary>
    /// Animates the page out
    /// </summary>
    /// <returns></returns>
    public async Task AnimateOutAsync()
    {
        // Make sure we have something to do
        if (PageUnloadAnimation == PageAnimation.None)
            return;

        switch (PageUnloadAnimation)
        {
            case PageAnimation.SlideAndFadeOutToLeft:

                // Start the animation
                await this.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, SlideSeconds);

                break;
        }
    }

    /// <summary>
    /// Fired when the view model changes
    /// </summary>
    protected virtual void OnViewModelChanged()
    {

    }
}

public abstract class BasePage<VM> : BasePage
    where VM : ObservableObject, new()
{
    /// <summary>
    /// The view model associated with this page
    /// </summary>
    public VM ViewModel
    {
        get => (VM)ViewModelObject;
        set => ViewModelObject = value;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public BasePage()
    {
        // If in design time mode...
        if (DesignerProperties.GetIsInDesignMode(this))
            // Just use a new instance of the VM
            ViewModel = new VM();
        else
            // Create a default view model
            ViewModel = Framework.Service<VM>() ?? new VM();
    }

    /// <summary>
    /// Constructor with specific view model
    /// </summary>
    /// <param name="specificViewModel">The specific view model to use, if any</param>
    public BasePage(VM? specificViewModel = null)
    {
        // Set specific view model
        if (specificViewModel is not null)
            ViewModel = specificViewModel;
        else
        {
            // If in design time mode...
            if (DesignerProperties.GetIsInDesignMode(this))
                // Just use a new instance of the VM
                ViewModel = new VM();
            else
            {
                // Create a default view model
                ViewModel = Framework.Service<VM>() ?? new VM();
            }
        }
    }
}

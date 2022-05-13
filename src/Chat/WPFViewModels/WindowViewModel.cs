using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.WPFViewModels;

/// <summary>
/// The View Model for the custom flat window
/// </summary>
public class WindowViewModel : ObservableObject
{
    /// <summary>
    /// The smallest width the window can go to
    /// </summary>
    public double WindowMinimumWidth { get; set; } = 800;

    /// <summary>
    /// The smallest height the window can go to
    /// </summary>
    public double WindowMinimumHeight { get; set; } = 500;

    /// <summary>
    /// The height of the title bar / caption of the window
    /// </summary>
    public int TitleHeight { get; set; } = 42;

    /// <summary>
    /// True if we should have a dimmed overlay on the window
    /// such as when a popup is visible or the window is not focused
    /// </summary>
    public bool DimmableOverlayVisible { get; set; }
}

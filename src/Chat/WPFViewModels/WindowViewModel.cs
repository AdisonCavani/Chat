using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.WPFViewModels;

/// <summary>
/// The View Model for the custom flat window
/// </summary>
public class WindowViewModel : ObservableObject
{
    public double WindowMinimumWidth { get; set; } = 800;

    public double WindowMinimumHeight { get; set; } = 500;

    public int TitleHeight { get; set; } = 42;

    public bool DimmableOverlayVisible { get; set; }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.WPFViewModels;

/// <summary>
/// The View Model for the custom flat window
/// </summary>
public partial class WindowViewModel : ObservableObject
{
    [ObservableProperty]
    private double windowMinimumWidth = 800;

    [ObservableProperty]
    private double windowMinimumHeight = 500;

    [ObservableProperty]
    private int titleHeight = 42;

    [ObservableProperty]
    private bool dimmableOverlayVisible;
}

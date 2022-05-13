using Chat.Core.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModels.Menu;

/// <summary>
/// A view model for a menu item
/// </summary>
public partial class MenuItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string? text;

    [ObservableProperty]
    private IconType icon;

    [ObservableProperty]
    private MenuItemType type;
}

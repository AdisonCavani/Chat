using Chat.Core.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.Menu;

/// <summary>
/// A view model for a menu item
/// </summary>
public class MenuItemViewModel : ObservableObject
{
    public string Text { get; set; }

    public IconType Icon { get; set; }

    public MenuItemType Type { get; set; }
}

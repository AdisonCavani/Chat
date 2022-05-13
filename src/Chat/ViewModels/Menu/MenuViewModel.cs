using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModels.Menu;

/// <summary>
/// A view model for a menu
/// </summary>
public partial class MenuViewModel : ObservableObject
{
    [ObservableProperty]
    private List<MenuItemViewModel>? items;
}

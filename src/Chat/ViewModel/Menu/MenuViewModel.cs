using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.Menu;

/// <summary>
/// A view model for a menu
/// </summary>
public class MenuViewModel : ObservableObject
{
    public List<MenuItemViewModel> Items { get; set; }
}

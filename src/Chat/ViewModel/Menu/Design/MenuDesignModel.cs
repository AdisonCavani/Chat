using Chat.Core.DataModels;

namespace Chat.ViewModel.Menu.Design;

public class MenuDesignModel : MenuViewModel
{
    public static MenuDesignModel Instance => new();

    public MenuDesignModel()
    {
        Items = new()
        {
            new() { Type = MenuItemType.Header, Text = "Design time header..." },
            new() { Text = "Menu item 1", Icon = IconType.File },
            new() { Text = "Menu item 2", Icon = IconType.Picture }
        };
    }
}

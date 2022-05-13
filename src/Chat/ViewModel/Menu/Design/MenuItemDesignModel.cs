using Chat.Core.DataModels;

namespace Chat.ViewModel.Menu.Design;

public class MenuItemDesignModel : MenuItemViewModel
{
    public static MenuItemDesignModel Instance => new();

    public MenuItemDesignModel()
    {
        Text = "Hello World";
        Icon = IconType.File;
    }
}

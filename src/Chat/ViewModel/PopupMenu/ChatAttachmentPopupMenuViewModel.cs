using Chat.Core.DataModels;
using Chat.ViewModel.Menu;

namespace Chat.ViewModel.PopupMenu;

/// <summary>
/// A view model for any popup menus
/// </summary>
public class ChatAttachmentPopupMenuViewModel : BasePopupViewModel
{
    public ChatAttachmentPopupMenuViewModel()
    {
        Content = new MenuViewModel()
        {
            Items = new()
            {
                new() { Text = "Attach a file...", Type = MenuItemType.Header },
                new() { Text = "From Computer", Icon = IconType.File },
                new() { Text = "From Pictures", Icon = IconType.Picture },
            }
        };
    }
}

using Chat.Core.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.PopupMenu;

public class BasePopupViewModel : ObservableObject
{
    public string BubbleBackground { get; set; }

    public ElementHorizontalAlignment ArrowAlignment { get; set; }

    public ObservableObject Content { get; set; }

    public BasePopupViewModel()
    {
        // Set default values
        // TODO: Move colors into Core and make use of it here
        BubbleBackground = "ffffff";
        ArrowAlignment = ElementHorizontalAlignment.Left;
    }
}

using Chat.Core.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModels.PopupMenu;

public partial class BasePopupViewModel : ObservableObject
{
    [ObservableProperty]
    private string? bubbleBackground;

    [ObservableProperty]
    private ElementHorizontalAlignment arrowAlignment;

    [ObservableProperty]
    private ObservableObject? content;

    public BasePopupViewModel()
    {
        // Set default values
        // TODO: Move colors into Core and make use of it here
        BubbleBackground = "ffffff";
        ArrowAlignment = ElementHorizontalAlignment.Left;
    }
}

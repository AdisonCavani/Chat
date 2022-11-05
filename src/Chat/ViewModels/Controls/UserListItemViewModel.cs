using CommunityToolkit.Mvvm.ComponentModel;
using Humanizer;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Chat.ViewModels.Controls;

public partial class UserListItemViewModel : ObservableObject
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(FullName))]
    string firstName;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(FullName))]
    string lastName;

    public string FullName => $"{firstName} {lastName}";

    public string LastMessage => Messages.Last().Message;

    public string Date => Messages.Last().Send.ToLocalTime().Humanize();

    public int UnreadMessages => Messages.Where(x => !x.Readed && !x.SendByMe).Count();

    public Brush DateForeground => UnreadMessages == 0
        ? new SolidColorBrush((Color)Application.Current.Resources["SystemColorGrayTextColor"])
        : new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColorLight2"]);

    public FontWeight DateFontWeight => UnreadMessages == 0
        ? FontWeights.Normal
        : FontWeights.SemiBold;

    public ObservableCollection<ChatMessageItemViewModel> Messages = new();
}

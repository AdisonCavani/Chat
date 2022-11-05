using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Chat.ViewModels.Controls;

public partial class ChatMessageItemViewModel : ObservableObject
{
    [ObservableProperty]
    string message;

    [ObservableProperty]
    bool readed;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(DisplayTime))]
    DateTime send = DateTime.UtcNow;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(DisplayTime))]
    DateTime delivered = DateTime.MinValue;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(DisplayTime))]
    DateTime seen = DateTime.MinValue;

    public string DisplayTime
    {
        get
        {
            if (Seen != DateTime.MinValue)
                return Seen.ToLocalTime().ToString("HH:mm");

            if (Delivered != DateTime.MinValue)
                return Delivered.ToLocalTime().ToString("HH:mm");

            return Send.ToLocalTime().ToString("HH:mm");
        }
    }

    public string FullDisplayTime
    {
        get
        {
            if (Seen != DateTime.MinValue)
                return Seen.ToLocalTime().ToLongDateString();

            if (Delivered != DateTime.MinValue)
                return Delivered.ToLocalTime().ToLongDateString();

            return Send.ToLocalTime().ToLongDateString();
        }
    }

    public bool SendByMe;

    public Brush BackgroundColor => SendByMe
        ? new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColorDark1"])
        : new SolidColorBrush(new Color()
        {
            A = 255,
            R = 54,
            G = 54,
            B = 54
        });

    public Brush ForegroundColor => SendByMe
        ? new SolidColorBrush(new()
        {
            A = 255,
            R = 205,
            G = 223,
            B = 219
        })
        : new SolidColorBrush(new()
        {
            A = 255,
            R = 215,
            G = 215,
            B = 215
        });

    public Brush SendHourForeground => SendByMe
        ? new SolidColorBrush(new()
        {
            A = 255,
            R = 95,
            G = 152,
            B = 142
        })
        : new SolidColorBrush(new()
        {
            A = 255,
            R = 118,
            G = 118,
            B = 118
        });

    public HorizontalAlignment HorizontalAlignment => SendByMe
        ? HorizontalAlignment.Right
        : HorizontalAlignment.Left;

    public Thickness Margin => SendByMe
        ? new(0, 2, 25, 0)
        : new(25, 2, 0, 0);

    #region Commands

    [ICommand]
    void CopyMessage()
    {
        var dataPackage = new DataPackage();
        dataPackage.SetText(Message);

        Clipboard.SetContent(dataPackage);
    }

    #endregion
}

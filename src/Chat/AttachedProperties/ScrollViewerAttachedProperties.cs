using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Chat.AttachedProperties;

public static class ScrollViewerAttachedProperties
{
    public static bool GetAutoScroll(DependencyObject obj)
    {
        return (bool)obj.GetValue(AutoScrollProperty);
    }

    public static void SetAutoScroll(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoScrollProperty, value);
    }

    public static readonly DependencyProperty AutoScrollProperty =
        DependencyProperty.RegisterAttached("ScrollToBottom", typeof(bool), typeof(ScrollViewerAttachedProperties), new PropertyMetadata(false, AutoScrollPropertyChanged));

    private static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var scrollViewer = d as ScrollViewer;

        if (scrollViewer != null && (bool)e.NewValue)
        {
            scrollViewer.ChangeView(null, scrollViewer.ScrollableHeight, null);
        }
    }
}
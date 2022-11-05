using Chat.ViewModels.Controls.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Chat.Controls;

public sealed partial class ChatMessage : UserControl
{
    public ChatMessage()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetRequiredService<UserListDesignViewModel>();

        Scroller.ViewChanged += Scroller_ViewChanged;
    }

    private void MessageTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter && Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down))
        {
            if (MessageTextBox.SelectionLength == 0)
            {
                var index = MessageTextBox.SelectionStart;

                MessageTextBox.Text = MessageTextBox.Text.Insert(index, "\r");
                MessageTextBox.SelectionStart = index + 1;
            }

            else
            {
                var index = MessageTextBox.SelectionStart;
                var length = MessageTextBox.SelectionLength;

                MessageTextBox.Text = new StringBuilder(MessageTextBox.Text).Remove(index, length).Insert(index, "\r").ToString();
                MessageTextBox.SelectionStart = index + 1;
            }

            e.Handled = true;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Scroller.ChangeView(null, Scroller.ScrollableHeight, null);
    }

    private void Scroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        if ((Scroller.ScrollableHeight - Scroller.VerticalOffset) <= 25)
            FadeInStoryboard.Begin();

        else
            FadeOutStoryboard.Begin();
    }
}

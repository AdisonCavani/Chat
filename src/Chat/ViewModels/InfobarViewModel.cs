using Chat.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace Chat.ViewModels;

public class InfobarViewModel : ObservableObject
{
    private readonly Infobar _infobar;

    public string Title => _infobar.Title;
    public string Message => _infobar.Message;
    public InfoBarSeverity Severity => _infobar.Severity;
    public bool Visible => _infobar.Visible;

    public InfobarViewModel(Infobar infobar)
    {
        _infobar = infobar;
    }
}

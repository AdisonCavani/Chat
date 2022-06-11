using Microsoft.UI.Xaml.Controls;

namespace Chat.Models;

public class Infobar
{
    public string Title { get; set; }

    public string Message { get; set; }

    public InfoBarSeverity Severity { get; set; }

    public bool Visible { get; set; }
}

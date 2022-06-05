using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chat.ViewModels;

public partial class SignalrViewModel : ObservableObject
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    string message;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    bool connectionOpened;

    public bool CanSend => !string.IsNullOrWhiteSpace(message) && ConnectionOpened;

    public ObservableCollection<string> Messages { get; private set; } = new();

    [ICommand]
    void OpenConnection()
    {

    }

    [ICommand]
    void SendMessage()
    {
        Message = string.Empty;
    }
}

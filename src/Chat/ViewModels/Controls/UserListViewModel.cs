using Chat.Services;
using Chat.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Chat.ViewModels.Controls;

public partial class UserListViewModel : ObservableObject
{
    private readonly Frame _frame;
    private readonly UserCredentialsManager _context;

    public UserListViewModel(Frame frame, UserCredentialsManager context)
    {
        _frame = frame;
        _context = context;
    }

    public ObservableCollection<UserListItemViewModel> Items { get; set; } = new();

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CurrentUser))]
    [AlsoNotifyChangeFor(nameof(UserStatus))]
    [AlsoNotifyChangeFor(nameof(UserFullName))]
    [AlsoNotifyChangeFor(nameof(UnreadMessages))]
    int? selectedItem = null;

    public string UserFullName => SelectedItem is null
        ? string.Empty
        : Items[SelectedItem.Value].FullName;

    public string UserStatus => SelectedItem is null
        ? string.Empty
        : $"Last seen {Items[SelectedItem.Value].Date}";

    public ObservableCollection<ChatMessageItemViewModel> CurrentUser => Items[SelectedItem.Value].Messages;

    public int UnreadMessages => Items[SelectedItem.Value].UnreadMessages;

    [ICommand]
    async Task Logout()
    {
        await _context.ClearUserCredentialsAsync();
        _frame.Navigate(typeof(LoginPage));
    }

    #region Old VM
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(SendButtonIcon))]
    string pendingMessage;

    public SymbolIcon SendButtonIcon => string.IsNullOrWhiteSpace(PendingMessage) ? new(Symbol.Microphone) : new(Symbol.Send);

    [ObservableProperty]
    string searchText;

    [ICommand]
    void Send()
    {
        if (string.IsNullOrWhiteSpace(PendingMessage))
        {
            // TODO: add microphone
            return;
        }

        SendMessage();
    }

    [ICommand]
    void EnterPressed()
    {
        var state = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift);

        if (state.HasFlag(CoreVirtualKeyStates.Down))
            return;

        if (string.IsNullOrWhiteSpace(PendingMessage))
            return;

        SendMessage();
    }

    void SendMessage()
    {
        var message = new ChatMessageItemViewModel()
        {
            Message = PendingMessage.Trim(),
            SendByMe = true
        };

        Items[SelectedItem.Value].Messages.Add(message);
        PendingMessage = string.Empty;
    }

    void Search()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
            return;

        var searchedWords = SearchText.ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in Items[SelectedItem.Value].Messages)
        {
            var msgWords = item.Message.ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries);

            var xd = msgWords.AsParallel().Any(x => searchedWords.AsParallel().Contains(x));
        }
    }

    #endregion
}

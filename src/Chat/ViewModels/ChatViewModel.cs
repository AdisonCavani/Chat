using Chat.Core;
using Chat.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly ILogger<ChatViewModel> _logger;
    private readonly UserCredentialsManager _context;
    private readonly ClientWebSocket _client;

    public ChatViewModel(ILogger<ChatViewModel> logger, UserCredentialsManager context)
    {
        _logger = logger;
        _context = context;

        _client = new();
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    string message;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    bool connectionOpened;

    public bool CanSend => !string.IsNullOrWhiteSpace(message);

    public ObservableCollection<string> Messages { get; private set; } = new();


    [ICommand]
    async void OpenConnection()
    {
        var credentials = await _context.GetUserCredentialsAsync();
        _client.Options.SetRequestHeader("Authorization", $"Bearer {credentials.Token}");

        try
        {
            await _client.ConnectAsync(new($"wss://localhost:5001/{ApiRoutes.Chat.Message.WebSocket}"), CancellationToken.None);

            ConnectionOpened = true;

            var receive = ReceiveAsync(_client);
            await Task.WhenAll(receive);
        }

        catch (Exception ex)
        {
            if (_client.State != WebSocketState.Open)
                ConnectionOpened = false;

            _logger.LogError(ex.Message);
        }
    }

    async Task ReceiveAsync(ClientWebSocket client)
    {
        var buffer = new byte[1024 * 4];

        while (true)
        {
            var result = await client.ReceiveAsync(new(buffer), CancellationToken.None);
            Messages.Add(Encoding.UTF8.GetString(buffer, 0, result.Count));

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                ConnectionOpened = false;
                break;
            }
        }
    }

    [ICommand]
    async Task SendMessage()
    {
        try
        {
            var send = Task.Run(async () =>
            {
                await _client.SendAsync(new(Encoding.UTF8.GetBytes(Message)), WebSocketMessageType.Text, true, CancellationToken.None);
                await _client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by client", CancellationToken.None);
            });

            await Task.WhenAll(send);
        }

        catch (Exception ex)
        {
            if (_client.State != WebSocketState.Open)
                ConnectionOpened = false;

            _logger.LogError(ex.Message);
        }

        Message = string.Empty;
    }
}

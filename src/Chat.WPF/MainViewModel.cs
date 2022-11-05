using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Chat.Core;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Chat.WPF;

public partial class MainViewModel : ObservableObject
{
    private readonly ClientWebSocket _client;

    public MainViewModel()
    {
        _client = new();
    }

    [ObservableProperty]
    string jwtToken = string.Empty;

    [ObservableProperty]
    bool connectionOpened;

    [ObservableProperty]
    string message = string.Empty;

    public ObservableCollection<string> Messages { get; private set; } = new();

    [ICommand]
    async void OpenConnection()
    {
        _client.Options.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        try
        {
            await _client.ConnectAsync(new($"wss://localhost:5001/{ApiRoutes.Chat.Message.WebSocket}"), CancellationToken.None);

            ConnectionOpened = true;

            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await _client.ReceiveAsync(new(buffer), CancellationToken.None);
                Messages.Add(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await Task.Delay(2000);

                    if (_client.State == WebSocketState.Open || _client.State == WebSocketState.CloseReceived)
                        await _client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

                    ConnectionOpened = false;
                    break;
                }
            }
        }

        catch (Exception ex)
        {
            await _client.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "", CancellationToken.None);
            ConnectionOpened = false;

            Debug.WriteLine(ex.Message);
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

            Debug.WriteLine(ex.Message);
        }

        Message = string.Empty;
    }
}

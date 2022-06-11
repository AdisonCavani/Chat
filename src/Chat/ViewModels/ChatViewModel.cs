using Chat.Core;
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

    public ChatViewModel(ILogger<ChatViewModel> logger)
    {
        _logger = logger;
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
    void OpenConnection()
    {

    }

    [ICommand]
    async Task SendMessage()
    {
        using var client = new ClientWebSocket();
        var uri = new Uri($"wss://localhost:5001/{ApiRoutes.Chat.Message.Send}");

        try
        {
            await client.ConnectAsync(uri, CancellationToken.None);

            // TODO: look at this while loop
            //while (client.State == WebSocketState.Open)
            if (client.State == WebSocketState.Open)
            {
                var bytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(Message));
                await client.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);

                var responseBuffer = new byte[1024];
                var offset = 0;
                var packet = 1024;

                while (true)
                {
                    var byteReceived = new ArraySegment<byte>(responseBuffer, offset, packet);
                    var response = await client.ReceiveAsync(byteReceived, CancellationToken.None);

                    var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                    Messages.Add(responseMessage);

                    if (response.EndOfMessage)
                        break;
                }
            }
        }

        catch (WebSocketException ex)
        {
            _logger.LogError(ex.Message);
        }

        Message = string.Empty;
    }
}

using Chat.Core;
using Chat.WebApi.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerBase = Chat.WebApi.Extensions.ControllerBase;

namespace Chat.WebApi.Controllers.Chat;

[ApiController]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;
    private readonly ChatHandler _socketHandler;

    public MessageController(ILogger<MessageController> logger, ChatHandler socketHandler)
    {
        _logger = logger;
        _socketHandler = socketHandler;
    }

    [HttpGet(ApiRoutes.Chat.Message.Connect)]
    public IActionResult Connect()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.WebSockets.AcceptWebSocketAsync();
        }

        return Ok(HttpStatusCode.SwitchingProtocols);
    }

    [HttpGet(ApiRoutes.Chat.Message.Send)]
    public async Task<IActionResult> Send()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Send(HttpContext, webSocket);
        }
        else
            return BadRequest();

        return Ok(HttpStatusCode.SwitchingProtocols);
    }

    async Task Send(HttpContext context, WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result =
            await webSocket.ReceiveAsync(new(buffer),
                CancellationToken.None); // TODO: use CancellationToken

        if (result is not null)
        {
            while (!result.CloseStatus.HasValue)
            {
                string message = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                await webSocket.SendAsync(
                    new(Encoding.UTF8.GetBytes(message)),
                    result.MessageType,
                    result.EndOfMessage, CancellationToken.None);
            }
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }

    [HttpGet(ApiRoutes.Chat.Message.WebSocket)]
    public async Task<IActionResult> WebSocketAsync([FromQuery] string username)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
            return BadRequest();

        using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _socketHandler.OnConnected(socket, username);

        await Receive(socket, async (result, buffer) =>
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var msg = _socketHandler.ReceiveString(result, buffer);
                await HandleMessage(socket, msg);
                return;
            }

            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await HandleDisconnect(socket);
                return;
            }
        });

        return Ok();
    }

    private async Task HandleDisconnect(WebSocket socket)
    {
        var disconnectedUser = await _socketHandler.OnDisconnected(socket);
        var disconnectMessage = new ServerMessage(disconnectedUser, true, _socketHandler.GetAllUsers());
        await _socketHandler.BroadcastMessage(JsonSerializer.Serialize(disconnectMessage));
    }

    private async Task HandleMessage(WebSocket socket, string message)
    {
        var clientMessage = TryDeserializeClientMessage(message);

        if (clientMessage is null)
            return;

        if (clientMessage.IsTypeConnection())
        {
            // For future improvements
        }

        else if (clientMessage.IsTypeChat())
        {
            var expectedUsername = _socketHandler.GetUsernameBySocket(socket);

            if (clientMessage.IsValid(expectedUsername))
            {
                var chatMessage = new ServerMessage(clientMessage);
                await _socketHandler.BroadcastMessage(JsonSerializer.Serialize(chatMessage));
            }
        }
    }

    private ClientMessage? TryDeserializeClientMessage(string str)
    {
        try
        {
            return JsonSerializer.Deserialize<ClientMessage>(str);
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error: invalid message format");
            return null;
        }
    }

    private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
        var buffer = new byte[1024 * 4];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            await HandleDisconnect(socket);
        }
    }
}
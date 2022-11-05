using Chat.Core;
using Chat.Core.Models;
using Chat.WebApi.Chat;
using Chat.WebApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerBase = Chat.WebApi.Extensions.ControllerBase;

namespace Chat.WebApi.Controllers.Chat;

// [Authorize]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ChatHandler _chatHandler;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ChatHandler chatHandler, UserManager<AppUser> userManager,
        ILogger<MessageController> logger)
    {
        _chatHandler = chatHandler;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet(ApiRoutes.Chat.Message.WebSocket)]
    public async Task<IActionResult> WebSocketAsync()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
            return BadRequest();

        var uid = HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (uid is null)
            return InternalServerError();

        var user = await _userManager.FindByIdAsync(uid);

        if (user is null)
            return InternalServerError();

        using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _chatHandler.OnConnected(socket, user);

        await Receive(socket, async (result, buffer) =>
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var msg = _chatHandler.ReceiveString(result, buffer);
                await HandleMessage(msg);
            }

            else if (result.MessageType == WebSocketMessageType.Close)
                await HandleDisconnect(socket);
        });

        return Ok();
    }

    private async Task HandleDisconnect(WebSocket socket)
    {
        var disconnectedUser = await _chatHandler.OnDisconnected(socket);
        var disconnectMessage = new ServerMessage(disconnectedUser, true, _chatHandler.GetAllUsers());
        await _chatHandler.BroadcastMessage(JsonSerializer.Serialize(disconnectMessage));
    }

    private async Task HandleMessage(string message)
    {
        var clientMessage = TryDeserializeClientMessage(message);

        if (clientMessage is null)
            return;

        if (clientMessage.Type == MessageType.CONNECTION)
        {
            // For future improvements
        }

        else if (clientMessage.Type == MessageType.CHAT)
        {
            var chatMessage = new ServerMessage(clientMessage);
            await _chatHandler.BroadcastMessage(JsonSerializer.Serialize(chatMessage));
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
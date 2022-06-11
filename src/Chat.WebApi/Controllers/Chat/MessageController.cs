using System;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chat.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = Chat.WebApi.Extensions.ControllerBase;

namespace Chat.WebApi.Controllers.Chat;

[ApiController]
public class MessageController : ControllerBase
{
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

                Debugger.Log(1, "", message);

                await webSocket.SendAsync(
                    new(Encoding.UTF8.GetBytes($"Server: {DateTime.UtcNow} | {message}")),
                    result.MessageType,
                    result.EndOfMessage, CancellationToken.None);
            }
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}
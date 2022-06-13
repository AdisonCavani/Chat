using Chat.WebApi.Models.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebApi.Chat;

public class ConnectionManager
{
    private static ConcurrentDictionary<string, WebSocket> _sockets = new();
    private static ConcurrentDictionary<AppUser, string> _users = new();

    public WebSocket GetSocketById(string id)
    {
        return _sockets.FirstOrDefault(p => p.Key == id).Value;
    }

    public ConcurrentDictionary<string, WebSocket> GetAllSockets()
    {
        return _sockets;
    }

    public List<AppUser> GetAllUsers()
    {
        return _users.Select(p => p.Key).ToList();
    }

    public string GetId(WebSocket socket)
    {
        return _sockets.FirstOrDefault(p => p.Value == socket).Key;
    }

    public AppUser? GetUserBySocketId(string socketId)
    {
        return _users.FirstOrDefault(p => p.Value == socketId).Key;
    }

    public AppUser? GetUserBySocket(WebSocket socket)
    {
        var socketId = GetId(socket);
        return GetUserBySocketId(socketId);
    }

    public void AddSocket(WebSocket socket)
    {
        var socketId = CreateConnectionId();
        _sockets.TryAdd(socketId, socket);
    }

    public void AddUser(WebSocket socket, AppUser user)
    {
        var socketId = GetId(socket);
        _users.TryAdd(user, socketId);
    }

    public async Task RemoveSocket(WebSocket socket, string description = "Connection closed")
    {
        var id = GetId(socket);
        if (!string.IsNullOrEmpty(id))
        {
            _sockets.TryRemove(id, out _);
        }

        if (socket.State != WebSocketState.Aborted)
        {
            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                statusDescription: description,
                                cancellationToken: CancellationToken.None);
        }
    }

    public void RemoveUser(AppUser user)
    {
        _users.TryRemove(user, out _);
    }

    public bool UsernameAlreadyExists(AppUser user)
    {
        return _users.ContainsKey(user);
    }

    private string CreateConnectionId()
    {
        return Guid.NewGuid().ToString();
    }
}
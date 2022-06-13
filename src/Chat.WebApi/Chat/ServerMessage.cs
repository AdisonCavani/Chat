using Chat.WebApi.Models.Entities;
using System.Collections.Generic;

namespace Chat.WebApi.Chat;

public class ServerMessage
{
    public ServerMessage()
    {
        Users = new();
    }

    public ServerMessage(MessageType messageType, string messageContent, List<AppUser> users)
    {
        Type = messageType.ToString();
        Content = messageContent;
        Users = users;
    }

    public ServerMessage(ClientMessage clientMessage)
    {
        Type = clientMessage.GetMessageType();
        Content = clientMessage.BuildChatMessageBody();
    }

    public ServerMessage(AppUser user, bool isDisconnect, List<AppUser> users)
    {
        Type = MessageType.CONNECTION.ToString();
        Content = BuildConnectionMessageBody(user, isDisconnect);
        Users = users;
    }

    public string? Type { get; set; }
    public string? Content { get; set; }
    public List<AppUser> Users { get; set; }

    private string BuildConnectionMessageBody(AppUser user, bool isDisconnect)
    {
        if (isDisconnect)
        {
            return $"User {user.FirstName} {user.LastName} left the room.";
        }
        else
        {
            return $"User {user.FirstName} {user.LastName} joined the room.";
        }
    }
}
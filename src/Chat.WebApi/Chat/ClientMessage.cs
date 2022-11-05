using Chat.Core.Models;

namespace Chat.WebApi.Chat;

public class ClientMessage
{
    public MessageType Type { get; set; }
    public string? Receiver { get; set; }
    public string? Content { get; set; }
    public bool? IsPrivate { get; set; }

    public string BuildChatMessageBody()
    {
        var receiver = string.IsNullOrWhiteSpace(Receiver) ? "Everybody" : Receiver;
        return $"Message to {receiver}: {Content}";
    }
}

namespace Chat.WebApi.Chat;

public class ClientMessage
{
    public string? Type { get; set; }
    public string? Receiver { get; set; }
    public string? Content { get; set; }
    public bool? IsPrivate { get; set; }

    public string BuildChatMessageBody()
    {
        var receiver = string.IsNullOrWhiteSpace(Receiver) ? "Everybody" : Receiver;
        return $"Message to {receiver}: {Content}";
    }

    public string GetMessageType()
    {
        return Type?.ToUpper() ?? string.Empty;
    }

    public bool IsTypeConnection()
    {
        return GetMessageType() == MessageType.CONNECTION.ToString();
    }

    public bool IsTypeChat()
    {
        return GetMessageType() == MessageType.CHAT.ToString();
    }
}

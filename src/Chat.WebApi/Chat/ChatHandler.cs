namespace Chat.WebApi.Chat;

public class ChatHandler : WebSocketHandler
{
    public ChatHandler(ConnectionManager connectionManager) : base(connectionManager)
    {
    }
}
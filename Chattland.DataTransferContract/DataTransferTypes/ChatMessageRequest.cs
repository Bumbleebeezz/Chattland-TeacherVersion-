namespace Chattland.DataTransferContract.DataTransferTypes;

//Todo: Konsolliderade ChatMessageRequest som fanns på flera ställen och lade den här
public class ChatMessageRequest
{
    public ChatMessage ChatMessage { get; set; }
    public string Room { get; set; }
}
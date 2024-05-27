using Chattland.DataTransferContract.ChatContracts;

namespace Chattland.DataTransferContract.DataTransferTypes;

public class ChatMessage : IChatMessage
{
    public MessageSender Sender { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
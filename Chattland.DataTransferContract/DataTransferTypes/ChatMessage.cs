using Chattland.DataTransferContract.ChatContracts;

namespace Chattland.DataTransferContract.DataTransferTypes;

//Todo: Flyttad från Chat-frontend till Chattland.DataTransferContract
public class ChatMessage : IChatMessage
{
	public MessageSender Sender { get; set; }
	public string Message { get; set; }
	public DateTime CreatedAt { get; set; }
}
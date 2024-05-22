using Chattland.DataTransferContract.DataTransferTypes;

namespace Chattland.DataTransferContract.ChatContracts;

public interface IChatMessage
{
    MessageSender Sender { get; set; }
    string Message { get; set; }
    DateTime CreatedAt { get; set; }
}
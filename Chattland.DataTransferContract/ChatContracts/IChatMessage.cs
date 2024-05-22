namespace Chattland.DataTransferContract.ChatContracts;

public interface IChatMessage
{
    IMessageSender Sender { get; set; }
    string Message { get; set; }
    DateTime CreatedAt { get; set; }
}
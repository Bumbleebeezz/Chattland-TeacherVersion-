using Chattland.DataTransferContract.ChatContracts;

namespace Chattland.DataTransferContract.DataTransferTypes;

public class MessageSenderDto : IMessageSender
{
    public string Name { get; set; }
}
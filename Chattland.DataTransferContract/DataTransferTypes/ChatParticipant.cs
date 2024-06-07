using Chattland.DataTransferContract.ChatContracts;

namespace Chattland.DataTransferContract.DataTransferTypes;

public class ChatParticipant : IChatParticipant
{
    public string Name { get; set; }
    public string ConnectionId { get; set; }
}
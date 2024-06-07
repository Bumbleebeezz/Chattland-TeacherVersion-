using Chattland.DataTransferContract.DataTransferTypes;

namespace Chattland.DataTransferContract.ChatContracts;

public interface IChatConversation
{
    public ICollection<ChatParticipant> Participants { get; set; }

    public ICollection<ChatMessage> Messages { get; set; }

    public DateTime LastMessageSent { get; set; }
}
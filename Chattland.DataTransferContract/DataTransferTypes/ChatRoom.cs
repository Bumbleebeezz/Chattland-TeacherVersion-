using Chattland.DataTransferContract.ChatContracts;

namespace Chattland.DataTransferContract.DataTransferTypes;

public class ChatRoom : IChatRoom
{
    public string Name { get; set; }
    public ICollection<ChatParticipant> Participants { get; set; }
    public DateTime LastMessageSent { get; set; }
}
using Chattland.DataTransferContract.DataTransferTypes;

namespace Chattland.DataTransferContract.ChatContracts;

public interface IChatRoom
{
    public string Name { get; set; }
    public ICollection<ChatParticipant> Participants { get; set; }
    public DateTime LastMessageSent { get; set; }
}
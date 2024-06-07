namespace Chattland.DataTransferContract.ChatContracts;

public interface IChatParticipant
{
    public string Name { get; set; }
    public string ConnectionId { get; set; }
}
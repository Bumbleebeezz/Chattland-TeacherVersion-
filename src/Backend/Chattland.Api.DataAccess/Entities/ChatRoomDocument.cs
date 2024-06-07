using Chattland.CommonInterfaces;
using Chattland.DataTransferContract.ChatContracts;
using Chattland.DataTransferContract.DataTransferTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chattland.Api.DataAccess.Entities;

public class ChatRoomDocument : BaseDocument, IChatRoom
{
    public string Name { get; set; }
    public ICollection<ChatParticipant> Participants { get; set; }
    public DateTime LastMessageSent { get; set; }
}
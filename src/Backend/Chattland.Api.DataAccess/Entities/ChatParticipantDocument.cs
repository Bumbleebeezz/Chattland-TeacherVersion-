using Chattland.CommonInterfaces;
using Chattland.DataTransferContract.ChatContracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chattland.Api.DataAccess.Entities;

public class ChatParticipantDocument : BaseDocument, IChatParticipant
{
    public string Name { get; set; }
    public string ConnectionId { get; set; }
}
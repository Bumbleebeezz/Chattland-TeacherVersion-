using Chattland.CommonInterfaces;
using Chattland.DataTransferContract.ChatContracts;
using Chattland.DataTransferContract.DataTransferTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chattland.Api.DataAccess.Entities;

public class ChatMessageDocument : IChatMessage, IEntity<string>
{
    [BsonRepresentation(BsonType.ObjectId), BsonId]
    public string Id { get; set; }

    //[JsonConverter(typeof(MessageSender))] <- OM man vill ha Sender som en IMessageSender
    public MessageSender Sender { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
using Chattland.CommonInterfaces;
using Chattland.DataTransferContract.ChatContracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chattland.Api.DataAccess.Entities;

public class ChatMessage : IChatMessage, IEntity<string>
{
    [BsonRepresentation(BsonType.ObjectId), BsonId]
    public string Id { get; set; }
    public IMessageSender Sender { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
using System.Text.Json.Serialization;
using Chattland.CommonInterfaces;
using Chattland.DataTransferContract.ChatContracts;
using Chattland.DataTransferContract.DataTransferTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chattland.Api.DataAccess.Entities;

public class ChatMessage : IChatMessage, IEntity<string>
{
    [BsonRepresentation(BsonType.ObjectId), BsonId]
    public string Id { get; set; }

    //[JsonConverter(typeof(MessageSenderDto))] <- OM man vill ha SenderDto som en IMessageSender
    public MessageSenderDto SenderDto { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
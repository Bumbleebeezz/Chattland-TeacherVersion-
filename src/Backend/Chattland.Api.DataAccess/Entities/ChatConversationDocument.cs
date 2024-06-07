using Chattland.CommonInterfaces;
using Chattland.DataTransferContract.ChatContracts;
using Chattland.DataTransferContract.DataTransferTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Chattland.Api.DataAccess.Entities;

public class ChatConversationDocument : BaseDocument, IChatConversation
{
    public ICollection<ChatParticipant> Participants { get; set; }
    public ICollection<ChatMessage> Messages { get; set; }
    public DateTime LastMessageSent { get; set; }
}
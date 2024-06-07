using Chattland.Api.DataAccess.Entities;
using Chattland.CommonInterfaces;

namespace Chattland.Api.DataAccess.Repositories.Interfaces;

public interface IChatConversationRepository : IRepository<ChatConversationDocument, string>
{

    Task<ChatRoomDocument> AddParticipantToConversation(string participantEmail, string roomName);
    Task<ChatRoomDocument> RemoveParticipantFromConversation(string participantEmail, string roomName);
}
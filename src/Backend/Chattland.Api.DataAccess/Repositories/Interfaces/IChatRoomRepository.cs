using Chattland.Api.DataAccess.Entities;
using Chattland.CommonInterfaces;
using Chattland.DataTransferContract.DataTransferTypes;

namespace Chattland.Api.DataAccess.Repositories.Interfaces;

public interface IChatRoomRepository : IRepository<ChatRoomDocument, string>
{
    Task<ChatRoomDocument> AddParticipantToRoom(string participantEmail, string roomName);
    Task<ChatRoomDocument> RemoveParticipantToRoom(string participantEmail, string roomName);
    Task<IEnumerable<string>> GetRoomNames();
}
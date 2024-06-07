using Chattland.Api.DataAccess.Entities;
using Chattland.CommonInterfaces;

namespace Chattland.Api.DataAccess.Repositories.Interfaces;

public interface IChatMessageRepository : IRepository<ChatMessageDocument, string>
{
    void SetCollectionName(string name);
}
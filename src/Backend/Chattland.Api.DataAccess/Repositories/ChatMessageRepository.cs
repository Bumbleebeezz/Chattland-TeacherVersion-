using Chattland.Api.DataAccess.Entities;
using Chattland.Api.DataAccess.Repositories.Interfaces;
using MongoDB.Driver;

namespace Chattland.Api.DataAccess.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly IMongoDatabase _database;
    private string _collectionName;
    private readonly IChatUnitOfWork _chatUnitOfWork;
    public ChatMessageRepository(IChatUnitOfWork chatUnitOfWork)
    {
        _chatUnitOfWork = chatUnitOfWork;

        IMongoClient client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase("Chattland");
        _collectionName = "Lobby";
    }

    public async Task<ChatMessageDocument> GetByIdAsync(string id)
    {
        var collection = _database.GetCollection<ChatMessageDocument>(_collectionName);
        var messages = await collection.FindAsync(id);
        var message = messages.FirstOrDefault();

        return message;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="count">Count set to 0 get all remaining messages</param>
    /// <returns>Collection of messages in specified range</returns>
    public async Task<IEnumerable<ChatMessageDocument>> GetManyAsync(int start, int count)
    {
        var collection = _database.GetCollection<ChatMessageDocument>(_collectionName);
        var messages = await collection.Find(_ => true).Skip(start).Limit(count).ToListAsync();
        return messages;
    }

    public async Task AddOneAsync(ChatMessageDocument item)
    {
        Action operation = async () =>
        {
            var collection =
                _database.GetCollection<ChatMessageDocument>(
                    _collectionName,
                    new MongoCollectionSettings()
                    {
                        AssignIdOnInsert = true
                    }
                );
            await collection.InsertOneAsync(item);
        };
        _chatUnitOfWork.AddOperation(operation);
    }

    public void SetCollectionName(string name)
    {
        _collectionName = name;
    }

    public async Task<IEnumerable<string>> GetRoomNames()
    {
        var collections = await _database.ListCollectionNamesAsync();
        var names = collections.ToList();
        return names;
    }
}
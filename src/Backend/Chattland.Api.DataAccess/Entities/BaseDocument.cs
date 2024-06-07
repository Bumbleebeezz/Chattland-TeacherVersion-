using Chattland.CommonInterfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chattland.Api.DataAccess.Entities;

public abstract class BaseDocument : IEntity<string>
{
    [BsonRepresentation(BsonType.ObjectId), BsonId]
    public string Id { get; set; }
}
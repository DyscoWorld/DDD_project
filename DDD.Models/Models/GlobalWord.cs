using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Models.Models;

/// <summary>
/// Слово в общем пуле
/// </summary>
public class GlobalWord
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Translation { get; set; }
    public string Definition { get; set; }
    public int Rank { get; set; }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApi.Models;

public class Drug
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;   

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("price")]
    public decimal Price { get; set; }   
}
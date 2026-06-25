using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApi.Models;

public class Pharmacy
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("drugs")]
    public List<PharmacyDrug> Drugs { get; set; } = new();
}
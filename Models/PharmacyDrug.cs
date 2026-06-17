using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApi.Models;

public class PharmacyDrug
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;   

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("manufactureDate")]
    public DateTime ManufactureDate { get; set; }

    [BsonElement("expiryDate")]
    public DateTime ExpiryDate { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }  

    [BsonElement("pharmacyNumber")]
    public int PharmacyNumber { get; set; }  

    [BsonElement("cost")]
    public decimal Cost { get; set; }   
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApi.Models;

public class PharmacyDrug
{
    [BsonElement("drugCode")]
    public string DrugCode { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("expiryDate")]
    public DateTime ExpiryDate { get; set; }
}
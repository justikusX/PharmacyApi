using MongoDB.Bson;
using MongoDB.Driver;
using PharmacyApi.Models;

namespace PharmacyApi.Services;

public class MongoDbService
{
    private readonly IMongoCollection<Drug> _drugs;
    private readonly IMongoCollection<PharmacyDrug> _pharmacyDrugs;

    public MongoDbService(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDb"));
        var database = client.GetDatabase("PharmacyDb");
        _drugs = database.GetCollection<Drug>("Drugs");
        _pharmacyDrugs = database.GetCollection<PharmacyDrug>("PharmacyDrugs");
    }

    
    public async Task<List<Drug>> GetDrugsAsync() =>
        await _drugs.Find(_ => true).ToListAsync();

    public async Task<List<PharmacyDrug>> GetPharmacyDrugsAsync() =>
        await _pharmacyDrugs.Find(_ => true).ToListAsync();

    
    public async Task InsertDrugsAsync(IEnumerable<Drug> drugs) =>
        await _drugs.InsertManyAsync(drugs);

    public async Task InsertPharmacyDrugsAsync(IEnumerable<PharmacyDrug> items) =>
        await _pharmacyDrugs.InsertManyAsync(items);

   
    public async Task<List<PharmacyWithPrice>> GetPharmaciesWithPriceByDrugCodeAsync(string drugCode)
    {
        
        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$match", new BsonDocument("code", drugCode)),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Drugs" },
                { "localField", "code" },
                { "foreignField", "code" },
                { "as", "drugInfo" }
            }),
            new BsonDocument("$unwind", "$drugInfo"),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$pharmacyNumber" },
                { "pharmacyNumber", new BsonDocument("$first", "$pharmacyNumber") },
                { "price", new BsonDocument("$first", "$drugInfo.price") }
            })
        };

        var result = await _pharmacyDrugs.Aggregate<PharmacyWithPrice>(pipeline).ToListAsync();
        return result;
    }

    
    public async Task<long> DeleteExpiredPharmacyDrugsAsync()
    {
        var filter = Builders<PharmacyDrug>.Filter.Lt(d => d.ExpiryDate, DateTime.UtcNow.Date);
        var result = await _pharmacyDrugs.DeleteManyAsync(filter);
        return result.DeletedCount;
    }

    
    public class PharmacyWithPrice
    {
        public int PharmacyNumber { get; set; }
        public decimal Price { get; set; }
    }
}
using MongoDB.Driver;
using PharmacyApi.Models;

namespace PharmacyApi.Services;

public class MongoDbService
{
    private readonly IMongoCollection<Pharmacy> _pharmacies;
    private readonly IMongoCollection<Drug> _drugs;

    public MongoDbService(IConfiguration config)
    {
        try
        {
            var connectionString = config.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = config.GetSection("MongoDB:DatabaseName").Value;

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
            {
                throw new Exception("MongoDB configuration is missing");
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

           
            _pharmacies = database.GetCollection<Pharmacy>("Pharmacies"); 
            _drugs = database.GetCollection<Drug>("Drugs");

            Console.WriteLine($" Connected to MongoDB: {databaseName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Error connecting to MongoDB: {ex.Message}");
            throw;
        }
    }

   

    public async Task<List<Pharmacy>> GetAllPharmaciesAsync()
    {
        return await _pharmacies.Find(_ => true).ToListAsync();
    }

    public async Task<Pharmacy> GetPharmacyByCodeAsync(string code)
    {
        return await _pharmacies.Find(p => p.Code == code).FirstOrDefaultAsync();
    }

    public async Task CreatePharmacyAsync(Pharmacy pharmacy)
    {
        await _pharmacies.InsertOneAsync(pharmacy);
    }

    public async Task UpdatePharmacyByCodeAsync(string code, Pharmacy pharmacy)
    {
        var filter = Builders<Pharmacy>.Filter.Eq(p => p.Code, code);
        await _pharmacies.ReplaceOneAsync(filter, pharmacy);
    }

    public async Task DeletePharmacyByCodeAsync(string code)
    {
        var filter = Builders<Pharmacy>.Filter.Eq(p => p.Code, code);
        await _pharmacies.DeleteOneAsync(filter);
    }

    

    public async Task<List<Drug>> GetAllDrugsAsync()
    {
        return await _drugs.Find(_ => true).ToListAsync();
    }

    public async Task<Drug> GetDrugByCodeAsync(string code)
    {
        return await _drugs.Find(d => d.Code == code).FirstOrDefaultAsync();
    }

    public async Task<Drug> GetDrugByIdAsync(string id)
    {
        return await _drugs.Find(d => d.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Drug>> GetDrugsByPharmacyCodeAsync(string pharmacyCode)
    {
        return await _drugs.Find(d => d.PharmacyCode == pharmacyCode).ToListAsync();
    }

    public async Task CreateDrugAsync(Drug drug)
    {
        await _drugs.InsertOneAsync(drug);
    }

    public async Task UpdateDrugAsync(string id, Drug drug)
    {
        var filter = Builders<Drug>.Filter.Eq(d => d.Id, id);
        await _drugs.ReplaceOneAsync(filter, drug);
    }

    public async Task DeleteDrugAsync(string id)
    {
        var filter = Builders<Drug>.Filter.Eq(d => d.Id, id);
        await _drugs.DeleteOneAsync(filter);
    }

    

    public async Task<List<PharmacyDrugInfo>> GetPharmacyDrugInfoAsync(string drugCode)
    {
        var pharmacies = await _pharmacies.Find(p => p.Drugs.Any(d => d.DrugCode == drugCode)).ToListAsync();

        var result = new List<PharmacyDrugInfo>();
        foreach (var pharmacy in pharmacies)
        {
            var drug = pharmacy.Drugs.FirstOrDefault(d => d.DrugCode == drugCode);
            if (drug != null)
            {
                result.Add(new PharmacyDrugInfo
                {
                    PharmacyCode = pharmacy.Code,
                    PharmacyName = pharmacy.Name,
                    DrugCode = drug.DrugCode,
                    DrugName = drug.Name,
                    Price = drug.Price,
                    Quantity = drug.Quantity,
                    ExpiryDate = drug.ExpiryDate
                });
            }
        }

        return result;
    }

    public async Task<int> DeleteExpiredDrugsAsync()
    {
        var currentDate = DateTime.UtcNow;

        var filter = Builders<Drug>.Filter.Lt(d => d.ExpiryDate, currentDate);
        var result = await _drugs.DeleteManyAsync(filter);
        int deletedCount = (int)result.DeletedCount;

        var pharmacies = await _pharmacies.Find(_ => true).ToListAsync();
        foreach (var pharmacy in pharmacies)
        {
            var expiredDrugs = pharmacy.Drugs.Where(d => d.ExpiryDate < currentDate).ToList();
            if (expiredDrugs.Any())
            {
                foreach (var expiredDrug in expiredDrugs)
                {
                    pharmacy.Drugs.Remove(expiredDrug);
                }
                await _pharmacies.ReplaceOneAsync(p => p.Id == pharmacy.Id, pharmacy);
            }
        }

        return deletedCount;
    }
}

public class PharmacyDrugInfo
{
    public string PharmacyCode { get; set; } = string.Empty;
    public string PharmacyName { get; set; } = string.Empty;
    public string DrugCode { get; set; } = string.Empty;
    public string DrugName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
}
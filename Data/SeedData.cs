using PharmacyApi.Models;
using PharmacyApi.Services;

namespace PharmacyApi.Data;

public static class SeedData
{
    public static async Task InitializeAsync(MongoDbService dbService)
    {
        
        var drugs = await dbService.GetDrugsAsync();
        if (drugs.Any()) return;

        
        var drugList = new List<Drug>
        {
            new Drug { Code = "D001", Name = "Аспирин", Price = 50 },
            new Drug { Code = "D002", Name = "Парацетамол", Price = 30 },
            new Drug { Code = "D003", Name = "Ибупрофен", Price = 80 },
            new Drug { Code = "D004", Name = "Но-шпа", Price = 120 },
            new Drug { Code = "D005", Name = "Анальгин", Price = 25 },
            new Drug { Code = "D006", Name = "Димедрол", Price = 45 },
            new Drug { Code = "D007", Name = "Корвалол", Price = 70 }
        };
        await dbService.InsertDrugsAsync(drugList);

        
        var random = new Random();
        var now = DateTime.UtcNow.Date;
        var pharmacyDrugs = new List<PharmacyDrug>();
        for (int i = 0; i < 15; i++)
        {
            var drug = drugList[random.Next(drugList.Count)];
            var manufacture = now.AddDays(-random.Next(0, 730));
            var expiry = manufacture.AddDays(random.Next(30, 730));
            pharmacyDrugs.Add(new PharmacyDrug
            {
                Code = drug.Code,
                Name = drug.Name,
                ManufactureDate = manufacture,
                ExpiryDate = expiry,
                Quantity = random.Next(5, 100),
                PharmacyNumber = random.Next(1, 8),
                Cost = drug.Price * random.Next(5, 20)
            });
        }
        await dbService.InsertPharmacyDrugsAsync(pharmacyDrugs);
    }
}
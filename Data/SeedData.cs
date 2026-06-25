using PharmacyApi.Models;
using PharmacyApi.Services;

namespace PharmacyApi.Data;

public static class SeedData
{
    public static async Task InitializeAsync(MongoDbService dbService)
    {
        try
        {
            var existingPharmacies = await dbService.GetAllPharmaciesAsync();
            var existingDrugs = await dbService.GetAllDrugsAsync();

            if (existingPharmacies.Any() && existingDrugs.Any())
            {
                Console.WriteLine(" Данные уже существуют, пропускаем инициализацию");
                return;
            }

            
            var drugs = new List<Drug>
            {
                new Drug { Code = "D001", Name = "Парацетамол", ManufactureDate = new DateTime(2025, 1, 15), ExpiryDate = new DateTime(2026, 1, 15), Quantity = 100, PharmacyCode = "P001", Cost = 5.99m },
                new Drug { Code = "D002", Name = "Ибупрофен", ManufactureDate = new DateTime(2025, 2, 10), ExpiryDate = new DateTime(2026, 2, 10), Quantity = 75, PharmacyCode = "P002", Cost = 7.49m },
                new Drug { Code = "D003", Name = "Амоксициллин", ManufactureDate = new DateTime(2025, 3, 5), ExpiryDate = new DateTime(2026, 3, 5), Quantity = 50, PharmacyCode = "P003", Cost = 12.99m },
                new Drug { Code = "D004", Name = "Лоратадин", ManufactureDate = new DateTime(2025, 4, 20), ExpiryDate = new DateTime(2026, 4, 20), Quantity = 60, PharmacyCode = "P004", Cost = 8.99m },
                new Drug { Code = "D005", Name = "Аспирин", ManufactureDate = new DateTime(2025, 5, 1), ExpiryDate = new DateTime(2026, 5, 1), Quantity = 200, PharmacyCode = "P001", Cost = 4.99m },
                new Drug { Code = "D006", Name = "Цитрамон", ManufactureDate = new DateTime(2025, 6, 15), ExpiryDate = new DateTime(2026, 6, 15), Quantity = 80, PharmacyCode = "P002", Cost = 6.49m },
                new Drug { Code = "D007", Name = "Но-шпа", ManufactureDate = new DateTime(2025, 7, 10), ExpiryDate = new DateTime(2026, 7, 10), Quantity = 45, PharmacyCode = "P003", Cost = 9.99m },
                new Drug { Code = "D008", Name = "Мезим", ManufactureDate = new DateTime(2025, 8, 5), ExpiryDate = new DateTime(2026, 8, 5), Quantity = 90, PharmacyCode = "P004", Cost = 11.49m },
                new Drug { Code = "D009", Name = "Фестал", ManufactureDate = new DateTime(2025, 9, 20), ExpiryDate = new DateTime(2026, 9, 20), Quantity = 55, PharmacyCode = "P005", Cost = 10.99m },
                new Drug { Code = "D010", Name = "Аллохол", ManufactureDate = new DateTime(2025, 10, 1), ExpiryDate = new DateTime(2026, 10, 1), Quantity = 70, PharmacyCode = "P006", Cost = 7.99m },
                new Drug { Code = "D011", Name = "Линекс", ManufactureDate = new DateTime(2025, 11, 15), ExpiryDate = new DateTime(2026, 11, 15), Quantity = 40, PharmacyCode = "P007", Cost = 15.99m },
                new Drug { Code = "D012", Name = "Эспумизан", ManufactureDate = new DateTime(2025, 12, 10), ExpiryDate = new DateTime(2026, 12, 10), Quantity = 65, PharmacyCode = "P001", Cost = 13.49m },
                new Drug { Code = "D013", Name = "Смекта", ManufactureDate = new DateTime(2024, 1, 5), ExpiryDate = new DateTime(2025, 1, 5), Quantity = 85, PharmacyCode = "P002", Cost = 8.49m },
                new Drug { Code = "D014", Name = "Регидрон", ManufactureDate = new DateTime(2024, 2, 20), ExpiryDate = new DateTime(2025, 2, 20), Quantity = 30, PharmacyCode = "P003", Cost = 6.99m },
                new Drug { Code = "D015", Name = "Панкреатин", ManufactureDate = new DateTime(2025, 3, 1), ExpiryDate = new DateTime(2026, 3, 1), Quantity = 95, PharmacyCode = "P004", Cost = 9.49m }
            };

         
            var pharmacies = new List<Pharmacy>
            {
                new Pharmacy
                {
                    Code = "P001",
                    Name = "Аптека №1 - Центральная",
                    Drugs = new List<PharmacyDrug>
                    {
                        new PharmacyDrug { DrugCode = "D001", Name = "Парацетамол", Price = 5.99m, Quantity = 25, ExpiryDate = new DateTime(2026, 1, 15) },
                        new PharmacyDrug { DrugCode = "D005", Name = "Аспирин", Price = 4.99m, Quantity = 40, ExpiryDate = new DateTime(2026, 5, 1) },
                        new PharmacyDrug { DrugCode = "D012", Name = "Эспумизан", Price = 13.49m, Quantity = 15, ExpiryDate = new DateTime(2026, 12, 10) }
                    }
                },
                new Pharmacy
                {
                    Code = "P002",
                    Name = "Аптека №2 - Северная",
                    Drugs = new List<PharmacyDrug>
                    {
                        new PharmacyDrug { DrugCode = "D002", Name = "Ибупрофен", Price = 7.49m, Quantity = 30, ExpiryDate = new DateTime(2026, 2, 10) },
                        new PharmacyDrug { DrugCode = "D006", Name = "Цитрамон", Price = 6.49m, Quantity = 20, ExpiryDate = new DateTime(2026, 6, 15) },
                        new PharmacyDrug { DrugCode = "D013", Name = "Смекта", Price = 8.49m, Quantity = 35, ExpiryDate = new DateTime(2025, 1, 5) }
                    }
                },
                new Pharmacy
                {
                    Code = "P003",
                    Name = "Аптека №3 - Южная",
                    Drugs = new List<PharmacyDrug>
                    {
                        new PharmacyDrug { DrugCode = "D003", Name = "Амоксициллин", Price = 12.99m, Quantity = 20, ExpiryDate = new DateTime(2026, 3, 5) },
                        new PharmacyDrug { DrugCode = "D007", Name = "Но-шпа", Price = 9.99m, Quantity = 15, ExpiryDate = new DateTime(2026, 7, 10) },
                        new PharmacyDrug { DrugCode = "D014", Name = "Регидрон", Price = 6.99m, Quantity = 10, ExpiryDate = new DateTime(2025, 2, 20) }
                    }
                },
                new Pharmacy
                {
                    Code = "P004",
                    Name = "Аптека №4 - Восточная",
                    Drugs = new List<PharmacyDrug>
                    {
                        new PharmacyDrug { DrugCode = "D004", Name = "Лоратадин", Price = 8.99m, Quantity = 25, ExpiryDate = new DateTime(2026, 4, 20) },
                        new PharmacyDrug { DrugCode = "D008", Name = "Мезим", Price = 11.49m, Quantity = 18, ExpiryDate = new DateTime(2026, 8, 5) },
                        new PharmacyDrug { DrugCode = "D015", Name = "Панкреатин", Price = 9.49m, Quantity = 22, ExpiryDate = new DateTime(2026, 3, 1) }
                    }
                },
                new Pharmacy
                {
                    Code = "P005",
                    Name = "Аптека №5 - Западная",
                    Drugs = new List<PharmacyDrug>
                    {
                        new PharmacyDrug { DrugCode = "D009", Name = "Фестал", Price = 10.99m, Quantity = 12, ExpiryDate = new DateTime(2026, 9, 20) }
                    }
                },
                new Pharmacy
                {
                    Code = "P006",
                    Name = "Аптека №6 - Пригородная",
                    Drugs = new List<PharmacyDrug>
                    {
                        new PharmacyDrug { DrugCode = "D010", Name = "Аллохол", Price = 7.99m, Quantity = 28, ExpiryDate = new DateTime(2026, 10, 1) }
                    }
                },
                new Pharmacy
                {
                    Code = "P007",
                    Name = "Аптека №7 - Детская",
                    Drugs = new List<PharmacyDrug>
                    {
                        new PharmacyDrug { DrugCode = "D011", Name = "Линекс", Price = 15.99m, Quantity = 8, ExpiryDate = new DateTime(2026, 11, 15) }
                    }
                }
            };

            
            foreach (var pharmacy in pharmacies)
            {
                await dbService.CreatePharmacyAsync(pharmacy);
            }

            foreach (var drug in drugs)
            {
                await dbService.CreateDrugAsync(drug);
            }

            Console.WriteLine($" Seed data added successfully: {pharmacies.Count} pharmacies, {drugs.Count} drugs");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Error seeding data: {ex.Message}");
            throw;
        }
    }
}
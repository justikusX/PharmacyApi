using Microsoft.AspNetCore.Mvc;
using PharmacyApi.Services;

namespace PharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PharmacyController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public PharmacyController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    
    [HttpGet("drug/{drugCode}")]
    public async Task<IActionResult> GetPharmaciesWithPrice(string drugCode)
    {
        var result = await _mongoDbService.GetPharmaciesWithPriceByDrugCodeAsync(drugCode);
        if (!result.Any())
            return NotFound($"Лекарство с кодом '{drugCode}' не найдено ни в одной аптеке.");

        return Ok(result);
    }

    
    [HttpDelete("expired")]
    public async Task<IActionResult> DeleteExpired()
    {
        var deletedCount = await _mongoDbService.DeleteExpiredPharmacyDrugsAsync();
        return Ok(new { deletedCount, message = "Удалены просроченные записи." });
    }
}
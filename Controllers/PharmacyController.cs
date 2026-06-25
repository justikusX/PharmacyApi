using Microsoft.AspNetCore.Mvc;
using PharmacyApi.Models;
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

   
    [HttpGet]
    public async Task<IActionResult> GetAllPharmacies()
    {
        try
        {
            var pharmacies = await _mongoDbService.GetAllPharmaciesAsync();
            return Ok(pharmacies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("{code}")]
    public async Task<IActionResult> GetPharmacyByCode(string code)
    {
        try
        {
            var pharmacy = await _mongoDbService.GetPharmacyByCodeAsync(code);
            if (pharmacy == null)
                return NotFound($"Pharmacy with code {code} not found");

            return Ok(pharmacy);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpPost]
    public async Task<IActionResult> CreatePharmacy([FromBody] Pharmacy pharmacy)
    {
        try
        {
            if (pharmacy == null)
                return BadRequest("Pharmacy cannot be null");

            var existing = await _mongoDbService.GetPharmacyByCodeAsync(pharmacy.Code);
            if (existing != null)
                return Conflict($"Pharmacy with code {pharmacy.Code} already exists");

            await _mongoDbService.CreatePharmacyAsync(pharmacy);
            return CreatedAtAction(nameof(GetPharmacyByCode), new { code = pharmacy.Code }, pharmacy);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

   
    [HttpPut("{code}")]
    public async Task<IActionResult> UpdatePharmacy(string code, [FromBody] Pharmacy pharmacy)
    {
        try
        {
            if (pharmacy == null || code != pharmacy.Code)
                return BadRequest("Invalid pharmacy data");

            var existing = await _mongoDbService.GetPharmacyByCodeAsync(code);
            if (existing == null)
                return NotFound($"Pharmacy with code {code} not found");

            await _mongoDbService.UpdatePharmacyByCodeAsync(code, pharmacy);
            return Ok(pharmacy);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpDelete("{code}")]
    public async Task<IActionResult> DeletePharmacy(string code)
    {
        try
        {
            var existing = await _mongoDbService.GetPharmacyByCodeAsync(code);
            if (existing == null)
                return NotFound($"Pharmacy with code {code} not found");

            await _mongoDbService.DeletePharmacyByCodeAsync(code);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("drugs")]
    public async Task<IActionResult> GetAllDrugs()
    {
        try
        {
            var drugs = await _mongoDbService.GetAllDrugsAsync();
            return Ok(drugs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("drugs/{drugCode}")]
    public async Task<IActionResult> GetDrugByCode(string drugCode)
    {
        try
        {
            var drug = await _mongoDbService.GetDrugByCodeAsync(drugCode);
            if (drug == null)
                return NotFound($"Drug with code {drugCode} not found");

            return Ok(drug);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpPost("drugs")]
    public async Task<IActionResult> CreateDrug([FromBody] Drug drug)
    {
        try
        {
            if (drug == null)
                return BadRequest("Drug cannot be null");

            await _mongoDbService.CreateDrugAsync(drug);
            return CreatedAtAction(nameof(GetDrugByCode), new { drugCode = drug.Code }, drug);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpPut("drugs/{id}")]
    public async Task<IActionResult> UpdateDrug(string id, [FromBody] Drug drug)
    {
        try
        {
            if (drug == null)
                return BadRequest("Drug cannot be null");

            var existing = await _mongoDbService.GetDrugByIdAsync(id);
            if (existing == null)
                return NotFound($"Drug with ID {id} not found");

            await _mongoDbService.UpdateDrugAsync(id, drug);
            return Ok(drug);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpDelete("drugs/{id}")]
    public async Task<IActionResult> DeleteDrug(string id)
    {
        try
        {
            var existing = await _mongoDbService.GetDrugByIdAsync(id);
            if (existing == null)
                return NotFound($"Drug with ID {id} not found");

            await _mongoDbService.DeleteDrugAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("pharmacy/{pharmacyCode}/drugs")]
    public async Task<IActionResult> GetDrugsByPharmacy(string pharmacyCode)
    {
        try
        {
            var drugs = await _mongoDbService.GetDrugsByPharmacyCodeAsync(pharmacyCode);
            return Ok(drugs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("drug/{drugCode}/pharmacies")]
    public async Task<IActionResult> GetPharmaciesWithDrug(string drugCode)
    {
        try
        {
            var result = await _mongoDbService.GetPharmacyDrugInfoAsync(drugCode);
            if (!result.Any())
                return NotFound($"No pharmacies found with drug {drugCode}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpDelete("expired")]
    public async Task<IActionResult> DeleteExpiredDrugs()
    {
        try
        {
            var deletedCount = await _mongoDbService.DeleteExpiredDrugsAsync();
            return Ok(new { Message = $"Deleted {deletedCount} expired drugs" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
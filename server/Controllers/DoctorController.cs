using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    [HttpGet(Name = "GetAllDoctors")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var doctors = await LoadAllDoctors();
             var sorted = doctors
                .OrderByDescending(d => d.Reviews.AverageRating)
                .ThenByDescending(d => d.Reviews.TotalRatings)
                .ThenBy(d => d.PromotionLevel)
                .ToList();

            return Ok(sorted);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:int}", Name = "GetDoctorById")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Id must be greater than 0.");

        try
        {
            var doctors = await LoadAllDoctors();
            var doctor = doctors.FirstOrDefault(d => d.Id == id);

            if (doctor == null)
                return NotFound($"Doctor with id {id} not found.");

            return Ok(doctor);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var doctors = await LoadAllDoctors();
            var active = doctors.Where(d => d.IsActive).ToList();
             var sorted = active
                .OrderByDescending(d => d.Reviews.AverageRating)
                .ThenByDescending(d => d.Reviews.TotalRatings)
                .ThenBy(d => d.PromotionLevel)
                .ToList();
                
            return Ok(sorted);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("promoted")]
    public async Task<IActionResult> GetPromoted()
    {
        try
        {
            var doctors = await LoadAllDoctors();
            var promoted = doctors.Where(d => d.PromotionLevel <= 5).ToList();
             var sorted = promoted
                .OrderByDescending(d => d.Reviews.AverageRating)
                .ThenByDescending(d => d.Reviews.TotalRatings)
                .ThenBy(d => d.PromotionLevel)
                .ToList();

            return Ok(sorted);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private async Task<List<Doctor>> LoadAllDoctors()
    {
        // בדיקה שהקבצים קיימים
        if (!System.IO.File.Exists("Data/doctors.json"))
            throw new FileNotFoundException("doctors.json not found.");

        if (!System.IO.File.Exists("Data/language.json"))
            throw new FileNotFoundException("language.json not found.");

        // קריאת רופאים
        var doctorsJson = await System.IO.File.ReadAllTextAsync("Data/doctors.json");
        var doctors = JsonSerializer.Deserialize<List<Doctor>>(doctorsJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception("Failed to deserialize doctors.json");

        // קריאת שפות
        var languagesJson = await System.IO.File.ReadAllTextAsync("Data/language.json");
        var languageData = JsonSerializer.Deserialize<LanguageData>(languagesJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception("Failed to deserialize language.json");

        var languagesDict = languageData.Languages ?? new Dictionary<string, string>();

        // המרה ממזהי שפות לשמות שפות
        foreach (var doctor in doctors)
        {
            doctor.LanguageIds = doctor.LanguageIds
                .Where(id => languagesDict.ContainsKey(id))
                .Select(id => languagesDict[id])
                .ToList();
        }

        return doctors;
    }
}
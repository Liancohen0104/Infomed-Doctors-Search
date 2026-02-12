using System.Reflection.Metadata.Ecma335;
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
        var doctors = await LoadAllDoctors();

        return Ok(doctors);
    }

    [HttpGet("{id:int}", Name = "GetDoctorById")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctors = await LoadAllDoctors();
        var doctor = doctors.FirstOrDefault(d => d.Id == id);

        if (doctor == null)
        {
            return NotFound();
        }

        return Ok(doctor);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var doctors = await LoadAllDoctors();
        var active = doctors.Where(d => d.IsActive).ToList();

        return Ok(active);
    }

    [HttpGet("promoted")]
    public async Task<IActionResult> GetPromoted()
    {
        var doctors = await LoadAllDoctors();
        var promoted = doctors.Where(d => d.PromotionLevel <= 5).ToList();

        return Ok(promoted);
    }

    [HttpGet("sorted")]
    public async Task<IActionResult> GetSorted()
    {
        var doctors = await LoadAllDoctors();

        var sorted = doctors
            .OrderByDescending(d => d.Reviews.AverageRating)
            .ThenByDescending(d => d.Reviews.TotalRatings)
            .ThenBy(d => d.PromotionLevel)
            .ToList();

        return Ok(sorted);
    }

    private async Task<List<Doctor>> LoadAllDoctors()
    {
        // קריאת רופאים
        var doctorsJson = await System.IO.File.ReadAllTextAsync("Data/doctors.json");
        var doctors = JsonSerializer.Deserialize<List<Doctor>>(doctorsJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Doctor>();

        // קריאת שפות
        var languagesJson = await System.IO.File.ReadAllTextAsync("Data/language.json");
        var languageData = JsonSerializer.Deserialize<LanguageData>(languagesJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var languagesDict = languageData?.Languages ?? new Dictionary<string, string>();

        // המרה ממזהי שפות לשמות שפות
        foreach (var doctor in doctors)
        {
            var names = doctor.LanguageIds
                .Where(id => languagesDict.ContainsKey(id))
                .Select(id => languagesDict[id])
                .ToList();

            doctor.LanguageIds = names; // החלפת הרשימה המקורית ברשימת שמות
        }

        return doctors;
    }
}

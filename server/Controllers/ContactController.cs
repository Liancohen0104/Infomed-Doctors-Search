using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private static readonly object FileLock = new();
    private const string FilePath = "Data/contact_requests.json";

    public record ContactRequestCreate(
        int DoctorId,
        string DoctorName,
        string FullName,
        string Phone,
        string Email
    );

    public record ContactRequestSaved(
        int Id,
        int DoctorId,
        string DoctorName,
        string FullName,
        string Phone,
        string Email,
        DateTime CreatedAt
    );

    [HttpPost]
    public IActionResult Create([FromBody] ContactRequestCreate req)
    {
        if (req.DoctorId <= 0) return BadRequest("doctorId must be > 0");
        if (string.IsNullOrWhiteSpace(req.DoctorName)) return BadRequest("doctorName is required");
        if (string.IsNullOrWhiteSpace(req.FullName)) return BadRequest("fullName is required");
        if (string.IsNullOrWhiteSpace(req.Phone)) return BadRequest("phone is required");
        if (string.IsNullOrWhiteSpace(req.Email)) return BadRequest("email is required");
        if (!req.Email.Contains("@")) return BadRequest("email is invalid");

        lock (FileLock)
        {
            Directory.CreateDirectory("Data");

            var existing = new List<ContactRequestSaved>();

            if (System.IO.File.Exists(FilePath))
            {
                var json = System.IO.File.ReadAllText(FilePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    existing = JsonSerializer.Deserialize<List<ContactRequestSaved>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    ) ?? new List<ContactRequestSaved>();
                }
            }

            var nextId = existing.Count == 0 ? 1 : existing.Max(x => x.Id) + 1;

            var saved = new ContactRequestSaved(
                Id: nextId,
                DoctorId: req.DoctorId,
                DoctorName: req.DoctorName,
                FullName: req.FullName,
                Phone: req.Phone,
                Email: req.Email,
                CreatedAt: DateTime.UtcNow
            );

            existing.Add(saved);

            var outJson = JsonSerializer.Serialize(existing, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            
            System.IO.File.WriteAllText(FilePath, outJson);

            return Ok(saved);
        }
    }
}
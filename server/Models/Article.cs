using System.Text.Json.Serialization;

namespace server.Models;

public class Article
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("sponsorships")]
    public List<Sponsorship> Sponsorships { get; set; } = new();
}

public class Sponsorship
{
    [JsonPropertyName("sponsorshipId")]
    public int SponsorshipId { get; set; }
}
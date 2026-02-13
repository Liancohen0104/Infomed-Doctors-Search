using System.Text.Json.Serialization;

namespace server.Models;

public class Doctor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("reviews")]
    public DoctorReviews Reviews { get; set; } = new();

    [JsonPropertyName("languageIds")]
    public List<string> LanguageIds { get; set; } = [];
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("promotionLevel")]
    public int PromotionLevel { get; set; }
    
    [JsonPropertyName("hasArticle")]
    public bool HasArticle { get; set; }
}

public class DoctorReviews
{
    [JsonPropertyName("averageRating")]
    public double AverageRating { get; set; }

    [JsonPropertyName("totalRatings")]
    public double TotalRatings { get; set; }
}

public class Phone
{
    [JsonPropertyName("number")]
    public string PhoneNumber { get; set; } = string.Empty;
}
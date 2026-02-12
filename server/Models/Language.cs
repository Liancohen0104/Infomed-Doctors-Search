using System.Text.Json.Serialization;

namespace server.Models;

public class LanguageData
{
    [JsonPropertyName("language")]
    public Dictionary<string, string> Languages { get; set; } = new();
}
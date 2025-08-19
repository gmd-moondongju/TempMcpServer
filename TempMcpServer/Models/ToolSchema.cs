using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TempMcpServer.Models
{
    public class ToolSchema
    {
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("properties")] public Dictionary<string, Property>? Properties { get; set; }
        [JsonPropertyName("required")] public string[]? Required { get; set; }
    }

    public class Property
    {
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
    }
}
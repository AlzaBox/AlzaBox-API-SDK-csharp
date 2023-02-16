using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

public class BaseResponse
{
    [JsonPropertyName("errors")]
    public List<Error>? Errors { get; set; }

    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }
}

public class Error
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("locations")]
    public List<string>? Locations { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("extensions")]
    public ErrorResponseExtension? Extensions { get; set; }
}

public class ErrorResponseExtension
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    
    [JsonPropertyName("translation")]
    public string? Translation { get; set; }    
}
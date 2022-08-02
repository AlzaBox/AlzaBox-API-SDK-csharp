using System.Text.Json.Serialization;

namespace ABAPI.Models;

public class ReservationsResponse
{
    [JsonPropertyName("data")]
    public List<Reservation>? Data { get; set; }

    public string? Errors { get; set; }
    public string? Metadata { get; set; }
}
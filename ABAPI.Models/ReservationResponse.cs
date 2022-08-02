using System.Text.Json.Serialization;

namespace ABAPI.Models;

public class ReservationResponse
{
    [JsonPropertyName("data")]
    public Reservation? Data { get; set; }

    public string? Errors { get; set; }
    public string? Metadata { get; set; }
}
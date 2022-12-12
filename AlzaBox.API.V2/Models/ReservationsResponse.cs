using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

public class ReservationsResponse
{
    [JsonPropertyName("data")]
    public ReservationsItems Data { get; set; }

    public string? Errors { get; set; }
    public string? Metadata { get; set; }
}

public class ReservationsItems
{
    [JsonPropertyName("reservations")]
    public List<Reservation>? Reservations { get; set; }
}
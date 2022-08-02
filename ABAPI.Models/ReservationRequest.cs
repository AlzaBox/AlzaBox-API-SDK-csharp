using System.Text.Json.Serialization;

namespace ABAPI.Models;

public class ReservationRequest
{
    [JsonPropertyName("data")]
    public ReservationRequestData? Data { get; set; }
}

public class ReservationRequestData
{
    [JsonPropertyName("reservation")]
    public Reservation? Reservation { get; set; }
}


using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

public class ReservationsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public ReservationsItems Data { get; set; }
}

public class ReservationsItems
{
    [JsonPropertyName("reservations")]
    public List<Reservation>? Reservations { get; set; }
}
using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

public class ReservationResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public Reservation? Data { get; set; }
}
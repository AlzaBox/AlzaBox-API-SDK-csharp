using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

public class VirtualBoxRequest
{
    public string state { get; set; }
}

public class VirtualBoxResponse
{
    [JsonPropertyName("timeStamp")]
    public string TimeStamp { get; set; }
    
    [JsonPropertyName("reservationId")]
    public string ReservationId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
}
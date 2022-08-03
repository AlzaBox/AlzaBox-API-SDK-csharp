using System.Net;
using System.Text.Json.Serialization;

namespace AlzaBox.API.Models;

public class BoxesResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<Box>? Data { get; set; }
}



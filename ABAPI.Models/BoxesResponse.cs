using System.Net;
using System.Text.Json.Serialization;

namespace ABAPI.Models;

public class BoxesResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<Box>? Data { get; set; }
}



using System.Net;
using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

public class BoxesResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public BoxesItems Data { get; set; }
    
    [JsonPropertyName("errors")]
    public string? Errors { get; set; }
    
    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }
}

public class BoxResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public BoxItem Data { get; set; }
    
    [JsonPropertyName("errors")]
    public string? Errors { get; set; }
    
    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }
}

public class BoxesItems
{
    [JsonPropertyName("boxes")]
    public List<Box>? Boxes { get; set; }
}

public class BoxItem
{
    [JsonPropertyName("box")]
    public Box? Box { get; set; }
}

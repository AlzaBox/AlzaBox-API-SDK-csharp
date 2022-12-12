namespace AlzaBox.API.WebExample.Data;

public class ChangeStatusRequest
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? RequestBody { get; set; }
    public string? IP { get; set; }
    public string? RequestHeader { get; set; }
}
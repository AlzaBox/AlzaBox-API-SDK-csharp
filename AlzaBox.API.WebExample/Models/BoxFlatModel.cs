namespace AlzaBox.API.WebExample.Models;

public class BoxFlatModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ZIP { get; set; }
    public string? StreetWithNumber { get; set; }
    public string? City { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public string? CountryShortCode { get; set; }
}
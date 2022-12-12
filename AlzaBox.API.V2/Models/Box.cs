using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

public class Box
{
    /// <summary>
    /// Locker identification 
    /// </summary>
    /// <example>123</example>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Locker type
    /// </summary>
    /// <example>16</example>    
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Locker attributes 
    /// </summary>
    [JsonPropertyName("attributes")]
    public BoxAttributes? Attributes { get; set; }
}

public class BoxAttributes
{
    /// <summary>
    /// Locker name 
    /// </summary>
    /// <example>Jankovcova 13</example>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Locker status 
    /// </summary>
    /// <example>true</example>
    [JsonPropertyName("available")]
    public bool Available { get; set; }

    /// <summary>
    /// Locker address 
    /// </summary>
    [JsonPropertyName("address")]
    public BoxAddress? Adress { get; set; } 

    /// <summary>
    /// Locker GPS position 
    /// </summary>
    [JsonPropertyName("gps")]
    public BoxGps? Gps { get; set; }

    /// <summary>
    /// Number of packages that can be reserved
    /// </summary>
    [JsonPropertyName("fittingPackages")]
    public List<BoxPackage>? FittingPackages { get; set; }
    
    /// <summary>
    /// Packages that cannot fit even the largest box
    /// </summary>
    [JsonPropertyName("tooLargePackages")]
    public List<BoxPackage>? TooLargePackages { get; set; }

    /// <summary>
    /// Slots that are required for package reservation
    /// </summary>
    [JsonPropertyName("requiredSlots")]
    public List<BoxRequiredSlot>? RequiredSlots { get; set; }

    /// <summary>
    /// Detailed description of box
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Urls of pictures, one main and several additional
    /// </summary>
    [JsonPropertyName("photos")]
    public BoxPhotos? Photos { get; set; }

    /// <summary>
    /// Locker opening hours 
    /// </summary>
    [JsonPropertyName("openingHours")]
    public List<BoxOpeningHours>? OpeningHours { get; set; }

    /// <summary>
    /// Sizes and counts of slots
    /// </summary>
    /// <example>123</example>
    [JsonPropertyName("slots")]
    public List<BoxSlots>? Slots { get; set; }

    /// <summary>
    /// General occupancy of box, physical plus reservations
    /// </summary>
    [JsonPropertyName("occupancy")]
    public float? Occupancy { get; set; }

    /// <summary>
    /// Partner's dynamic delivery PIN, which can be used on all AlzaBoxes if courier does not have any packages. It is valid until midnight.
    /// </summary>
    /// <example>123456</example>
    [JsonPropertyName("deliveryPin")]
    public string? DeliveryPin { get; set; }
    
    /// <summary>
    /// Short code of country where box is situated in ISO 3166-1alpha2 format  
    /// </summary>
    /// <example>cz</example>
    public string? CountryShortCode { get; set; }
}

public class BoxAddress
{
    [JsonPropertyName("streetWithNumber")]
    public string? StreetWithNumber { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }
}

public class BoxGps
{
    [JsonPropertyName("lat")]
    public double? Lat { get; set; }

    [JsonPropertyName("lng")]
    public double? Lng { get; set; }
}

public class BoxPackage
{
    [JsonPropertyName("height")]
    public float? Height { get; set; }

    [JsonPropertyName("depth")]
    public float? Depth { get; set; }

    [JsonPropertyName("width")]
    public float? Width { get; set; }
}

public class BoxRequiredSlot
{
    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; set; }

    [JsonPropertyName("availableCount")]
    public int? AvailableCount { get; set; }

    [JsonPropertyName("maxCount")]
    public int? MaxCount { get; set; }
}

public class BoxPhotos
{
    /// <summary>
    /// Url of main picture
    /// </summary>
    /// <example></example>        
    [JsonPropertyName("main")]
    public string? Main { get; set; }

    /// <summary>
    /// Aditional urls of pictures
    /// </summary>
    /// <example></example>     
    [JsonPropertyName("additional")]
    public List<string>? Additional { get; set; }
}

public class BoxOpeningHours
{
    [JsonPropertyName("day")]
    public string? Day { get; set; }

    [JsonPropertyName("open")]
    public string? Open { get; set; }

    [JsonPropertyName("close")]
    public string? Close { get; set; }
}

public class BoxSlots
{
    [JsonPropertyName("depth")]
    public double? Depth { get; set; }

    [JsonPropertyName("height")]
    public double? Height { get; set; }

    [JsonPropertyName("width")]
    public double? Width { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("slotId")]
    public int? SlotId { get; set; }
}

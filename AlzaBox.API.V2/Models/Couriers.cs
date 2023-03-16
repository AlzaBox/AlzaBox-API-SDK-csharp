using System.Text.Json.Serialization;

namespace AlzaBox.API.V2.Models;

/// <summary>
/// Response for GetCourier endpoint
/// </summary>
public class GetCourierResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public CourierDataWithOneCourierItemWithAttributes Data { get; set; }
}

public class GetCouriersResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public CourierDataWithManyCourierItems Data { get; set; }
}

/// <summary>
/// Input for CreateCourier or UpdateCourier endpoint
/// </summary>
public class CreateOrUpdateCourierRequest
{
    /// <summary>
    /// Courier's data
    /// </summary>
    [JsonPropertyName("data")]
    public CourierDataWithOneCourierItemWithoutAttributes Data { get; set; } = null!;
}

public class CreateOrUpdateCourierResponse : BaseResponse
{
    /// <summary>
    /// Courier's data
    /// </summary>
    [JsonPropertyName("data")]
    public CourierWithAttributes Data { get; set; }
}

public class CourierDataWithOneCourierItemWithAttributes
{
    /// <summary>
    /// Courier information
    /// </summary>
    [JsonPropertyName("courier")]
    public CourierWithAttributes Courier { get; set; } = null!;
}

/// <summary>
/// Data of GetCourier Response
/// </summary>
public class CourierDataWithOneCourierItemWithoutAttributes
{
    /// <summary>
    /// Courier information
    /// </summary>
    [JsonPropertyName("courier")]
    public CourierWithoutAttributes Courier { get; set; } = null!;
}

/// <summary>
/// ResponseData for GetCouriers endpoint
/// </summary>
public class CourierDataWithManyCourierItems
{
    /// <summary>
    /// Couriers information
    /// </summary>
    [JsonPropertyName("couriers")]
    public List<CourierWithAttributes> Couriers { get; set; } 
}

/// <summary>
/// Box access for courier creation or update
/// </summary>
public class CourierBox
{
    /// <summary>
    /// AlzaBox Id
    /// </summary>
    /// <example>1234</example>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// AlzaBox Partner Id
    /// </summary>
    /// <example>PID9876</example>
    [JsonPropertyName("pid")]
    public string? Pid { get; set; }
}

/// <summary>
/// Courier response
/// </summary>
public class CourierWithAttributes
{
    /// <summary>
    /// Login of the courier
    /// </summary>
    /// <example>JohnDoe123</example>
    [JsonPropertyName("login")]
    public string Login { get; set; } = null!;

    /// <summary>
    /// Pin for AlzaBox access
    /// </summary>
    /// <example>123456</example>
    [JsonPropertyName("pin")]
    public string? Pin { get; set; }
    
    /// <summary>
    /// Courier's attributes
    /// </summary>
    [JsonPropertyName("attributes")]
    public CourierAttributes Attributes { get; set; } = null!;
}

public class CourierWithoutAttributes
{
    /// <summary>
    /// Login of the courier
    /// </summary>
    /// <example>JohnDoe123</example>
    [JsonPropertyName("login")]
    public string Login { get; set; } = null!;

    /// <summary>
    /// Pin for AlzaBox access
    /// </summary>
    /// <example>123456</example>
    [JsonPropertyName("pin")]
    public string? Pin { get; set; }
    
    /// <summary>
    /// Boxes access type
    /// </summary>
    /// <example>SPECIFIC</example>
    [JsonPropertyName("boxesAccessType")]
    public string BoxesAccessType { get; set; }
    

    /// <summary>
    /// Boxes to which courier has access
    /// </summary>
    [JsonPropertyName("boxes")]
    public List<CourierBox> Boxes { get; set; }
}

/// <summary>
/// Courier response attributes
/// </summary>
public class CourierAttributes
{
    /// <summary>
    /// Boxes access type
    /// </summary>
    /// <example>SPECIFIC</example>
    [JsonPropertyName("boxesAccessType")]
    public string BoxesAccessType { get; set; }
    
    
    /// <summary>
    /// Boxes to which courier has access
    /// </summary>
    [JsonPropertyName("boxes")]
    public List<CourierBox> Boxes { get; set; }
}
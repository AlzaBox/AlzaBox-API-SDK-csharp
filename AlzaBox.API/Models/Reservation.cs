using System.Text.Json.Serialization;

namespace AlzaBox.API.Models;

public class Reservation
{
    /// <summary>
    /// Unique identification of reservation which must generate partner
    /// </summary>
    /// <example>ID987654</example>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("attributes")]
    public ReservationRequestAttributes? Attributes { get; set; }

    [JsonPropertyName("relationships")]
    public ReservationRequestRelationships? Relationships { get; set; }
}

public class ReservationRequestAttributes
{
    /// <summary>
    /// Status of reservation
    /// 
    /// RESERVED -
    /// 
    /// CANCELED - 
    /// </summary>
    /// <example>CANCELED</example>    
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Date 
    /// 
    /// </summary>
    /// <para>Test</para>
    /// <example>ID987654</example>
    [JsonPropertyName("expiration_date")]
    public string? ExpirationDate { get; set; }

    [JsonPropertyName("paymentData")]
    public ReservationRequestPaymentData? PaymentData { get; set; }

    [JsonPropertyName("packages")]
    public List<ReservationRequestPackages>? Packages { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class ReservationRequestPaymentData
{
    [JsonPropertyName("price")]
    public float Price { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("variableSymbo")]
    public string? VariableSymbo { get; set; }
}

public class ReservationRequestPackages
{
    [JsonPropertyName("depth")]
    public float Depth { get; set; }

    [JsonPropertyName("height")]
    public float Height { get; set; }

    [JsonPropertyName("width")]
    public float Width { get; set; }

    [JsonPropertyName("barCode")]
    public string? BarCode { get; set; }

    [JsonPropertyName("packageState")]
    public string? PackageState { get; set; }

    [JsonPropertyName("shippingList")]
    public ReservationRequestShippingList? ShippingList { get; set; }
}

public class ReservationRequestShippingList
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }
    
    [JsonPropertyName("carrier")]
    public ReservationRequestCarrier? Carrier { get; set; }
}

public class ReservationRequestCarrier
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class ReservationRequestRelationships
{
    [JsonPropertyName("box")]
    public ReservationRequestBox? Box { get; set; }
}

public class ReservationRequestBox
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
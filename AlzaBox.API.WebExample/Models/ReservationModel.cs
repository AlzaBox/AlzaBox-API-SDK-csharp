using System.ComponentModel.DataAnnotations;

namespace AlzaBox.API.WebExample.Models;

public class ReservationModel
{
    [Display(Name = "Reservation Id")] 
    public string? Id { get; set; }
    
    [Display(Name = "Package number")] 
    public string? PackageNumber { get; set; }

    [Display(Name = "AlzaBox number")] 
    public int BoxId { get; set; }
    
    [Display(Name = "Expiration date")]
    [DisplayFormat(DataFormatString = "{dd.MM.yyyy HH:mm}")]
    public DateTime ExpirationDate { get; set; }
    
    public float? Width { get; set; }
    
    public float? Height { get; set; }
    
    public float? Depth { get; set; }
    
   
}


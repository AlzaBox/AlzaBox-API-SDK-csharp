using System.ComponentModel.DataAnnotations;

namespace AlzaBox.API.WebExample.Models;

public class VirtualBoxModel
{
    [Display(Name = "Reservation Id")] 
    public string? Id { get; set; }
    
    [Display(Name = "Package number")] 
    public string? PackageNumber { get; set; }

    [Display(Name = "AlzaBox number")] 
    public int BoxId { get; set; }
    
    [Display(Name = "Virtual state")]
    public string State { get; set; }
}
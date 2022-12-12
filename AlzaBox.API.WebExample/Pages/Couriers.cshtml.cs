using AlzaBox.API.WebExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlzaBox.API.WebExample.Pages;

public class Couriers : BasePageModel
{

    private readonly ILogger<IndexModel> _logger;
    private readonly ABAPIService _abapi;

    public List<AlzaBox.API.Models.CourierWithAttributes>? CouriersData { get; set; }
    
    public Couriers(ILogger<IndexModel> logger, ABAPIService abapiService)
    {
        _abapi = abapiService; 
        _abapi.IsSignedIn(true);
    }
    
    public async Task<IActionResult> OnGet()
    {
        CouriersData = new List<AlzaBox.API.Models.CourierWithAttributes>();
        try
        {
            var boxes = await _abapi.client.Couriers.Get();
            CouriersData = boxes.Data.Couriers;
        }
        catch (HttpRequestException ex)
        {
            SetFlash(FlashMessageType.Danger, ex.Source + ex.Message);
        }

        return Page();
    }    

    
}
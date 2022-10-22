using AlzaBox.API.WebExample.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlzaBox.API.WebExample.Pages;

public class Label : BasePageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty] 
    public Models.ReservationModel Entity { get; set; }

    public string? Action;

    public AlzaBox.API.Models.Box? BoxData { get; set; }
    private readonly ABAPIService _abapi;

    
    public Label(ILogger<IndexModel> logger, ABAPIService abapiService)
    {
        _logger = logger;
        _abapi = abapiService;
        _abapi.IsSignedIn(true);
    }

    
    public async Task<IActionResult> OnGet(string id)
    {
        try
        {
            var reservationsResponse = await _abapi.client.Reservations.Get(id);
            var reservation = reservationsResponse.Data.FirstOrDefault();
            Entity = Entity.Map(reservation);
            var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
            BoxData = boxes.Data[0];
        }
        catch (Exception ex)
        {
            SetFlash(FlashMessageType.Danger, ex.Source + ex.Message);
        }
        
        return Page();
    }
}
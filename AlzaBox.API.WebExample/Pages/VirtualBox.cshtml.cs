using AlzaBox.API.WebExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AlzaBox.API.V2.Clients;
using AlzaBox.API.V2.Models;

namespace AlzaBox.API.WebExample.Pages;

public class VirtualBox : BasePageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ABAPIService _abapi;
    public string? Action;
    
    [BindProperty] 
    public Models.VirtualBoxModel Entity { get; set; }

    public VirtualBox(ILogger<IndexModel> logger, ABAPIService abapiService)
    {
        _logger = logger;
        _abapi = abapiService;
        _abapi.IsSignedIn(true);
    }
    
    public void OnGet()
    {
        Entity = new VirtualBoxModel();
    }

    public async Task<IActionResult> OnPostStock()
    {
        try
        {
            _abapi.clientV2.VirtualBox.Stocked(Entity.Id);
            SetFlash(FlashMessageType.Info, "Reservation was stocked");
        }
        catch (Exception ex)
        {
            SetFlash(FlashMessageType.Warning, $"VirtualBox error: {ex.Message}");
            _logger.LogError($"VirtualBox error: {ex.Message}", ex);
        }
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostCustomerPickup()
    {
        try
        {
            _abapi.clientV2.VirtualBox.PickedUp(Entity.Id);
            SetFlash(FlashMessageType.Info, "Reservation PickedUp");
        }
        catch (Exception ex)
        {
            SetFlash(FlashMessageType.Warning, $"VirtualBox error: {ex.Message}");
            _logger.LogError($"VirtualBox error: {ex.Message}", ex);
        }

        return Page();
    }
}
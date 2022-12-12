using AlzaBox.API.Models;
using AlzaBox.API.WebExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlzaBox.API.WebExample.Pages;

public class Courier : BasePageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ABAPIService _abapi;

    [BindProperty] public API.Models.CourierWithAttributes Entity { get; set; }
    
    [BindProperty] public AddBoxModel AddBoxEntity { get; set; }

    public string? Action;

    public Courier(ILogger<IndexModel> logger, ABAPIService abapiService)
    {
        _logger = logger;
        _abapi = abapiService;
        _abapi.IsSignedIn(true);
    }

    public async Task<IActionResult> OnGetCreateCourier()
    {

        Entity = new API.Models.CourierWithAttributes()
        {
        };

        SetFlash(FlashMessageType.Info, "Creating courier");
        Action = "Create";
        return Page();
    }
    
    public async Task<IActionResult> OnGetUpdateCourier(string login)
    {
        var couriersResponse = await _abapi.client.Couriers.Get(login);
        Entity = couriersResponse.Data.Couriers.FirstOrDefault();

        SetFlash(FlashMessageType.Info, "Updating courier");
        Action = "Update";
        return Page();
    }
    
    public async Task<IActionResult> OnGetAddBox(string login)
    {
        var couriersResponse = await _abapi.client.Couriers.Get(login);
        Entity = couriersResponse.Data.Couriers.FirstOrDefault();

        SetFlash(FlashMessageType.Info, "Updating courier");
        Action = "AddBox";
        return Page();
    }
    
    public async Task<IActionResult> OnGetRemoveBox(string login, int boxId)
    {
        var couriersResponse = await _abapi.client.Couriers.Get(login);
        var courier = couriersResponse.Data.Couriers.FirstOrDefault();
        var boxes = courier.Attributes.Boxes;
        var boxToRemove = boxes.FirstOrDefault(x => x.Id == boxId);
        boxes.Remove(boxToRemove);
        var updateResponse = await _abapi.client.Couriers.Update(courier.Login, courier.Pin, boxes);

        SetFlash(FlashMessageType.Info, "Removing box");
        
        return RedirectToPage("/courier", new { login = login });
    }
    
    public async Task<IActionResult> OnGet(string login)
    {
        try
        {
            var courierResponse = await _abapi.client.Couriers.Get(login);
            Entity = courierResponse.Data.Couriers.FirstOrDefault();
        }
        catch (Exception ex)
        {
            SetFlash(FlashMessageType.Danger, ex.Source + ex.Message);
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        var response = await _abapi.client.Couriers.Create(Entity.Login, Entity.Pin, null);

        if (response.Data != null)
        {
            Entity = response.Data;
            SetFlash(FlashMessageType.Info, "Courier created");
        }
        else
        {
            //Entity = new API.Models.Courier()
            SetFlash(FlashMessageType.Danger, response.Metadata);
        }

        return Page();
    }    
    
    public async Task<IActionResult> OnPostUpdate()
    {
        var response = await _abapi.client.Couriers.Update(Entity.Login, Entity.Pin, null);

        if (response.Data != null)
        {
            Entity = response.Data;
            SetFlash(FlashMessageType.Info, "Courier updated");
        }
        else
        {
            //Entity = new API.Models.Courier()
            SetFlash(FlashMessageType.Danger, response.Metadata);
        }
        
        Action = "Update";
        return Page();
    }
    
    public async Task<IActionResult> OnPostAddBox()
    {
        var couriersResponse = await _abapi.client.Couriers.Get(AddBoxEntity.Login);
        var boxes = couriersResponse.Data.Couriers.FirstOrDefault().Attributes.Boxes;
        if (boxes == null)
        {
            boxes = new List<CourierBox>();
        }
        
        boxes.Add(new CourierBox()
        {
            Id = AddBoxEntity.BoxId
        });
        
        var response = await _abapi.client.Couriers.Update(Entity.Login, Entity.Pin, boxes);

        if (response.Data != null)
        {
            Entity = response.Data;
            SetFlash(FlashMessageType.Info, "Box added");
        }
        else
        {
            //Entity = new API.Models.Courier()
            SetFlash(FlashMessageType.Danger, response.Metadata);
        }
        
        Action = "Update";
        return Page();
    }    
}
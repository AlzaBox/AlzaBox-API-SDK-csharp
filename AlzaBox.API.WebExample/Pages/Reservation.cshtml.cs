
using AlzaBox.API.WebExample.Core.Extensions;
using AlzaBox.API.WebExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlzaBox.API.WebExample.Pages;

public class Reservation : BasePageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty] public Models.ReservationModel Entity { get; set; }

    public string? Action;

    public AlzaBox.API.Models.Box? BoxData { get; set; }
    private readonly ABAPIService _abapi;

    public Reservation(ILogger<IndexModel> logger, ABAPIService abapiService)
    {
        _logger = logger;
        _abapi = abapiService;
        _abapi.IsSignedIn(true);
    }
    
    public async Task<IActionResult> OnGet(string id)
    {
        try
        {
            Entity = Entity.Map(await _abapi.client.Reservations.Get(id));
            var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
            BoxData = boxes.Data[0];
        }
        catch (Exception ex)
        {
            SetFlash(FlashMessageType.Danger, ex.Source + ex.Message);
        }
        
        return Page();
    }

    public async Task<IActionResult> OnGetCancel(string id)
    {
        var reservationResponse = await _abapi.client.Reservations.Cancel(id);
        if (reservationResponse.Data != null)
        {
            Entity = Entity.Map(reservationResponse.Data);
            var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
            BoxData = boxes.Data[0];
            SetFlash(FlashMessageType.Info, "Reservation was cancelled");
        }
        else
        {
            var reservationResponse1 = await _abapi.client.Reservations.Get(id);
            Entity = Entity.Map(reservationResponse1);
            var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
            BoxData = boxes.Data[0];
            SetFlash(FlashMessageType.Danger, reservationResponse.Metadata);
        }

        return Page();
    }

    public async Task<IActionResult> OnGetExtend(string id)
    {
        try
        {
            var reservation = await _abapi.client.Reservations.Get(id);
            Entity = Entity.Map(reservation);
            var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
            BoxData = boxes.Data[0];
            Action = "Extend";
            SetFlash(FlashMessageType.Info, "Extending reservation");
        }
        catch (Exception ex)
        {
            SetFlash(FlashMessageType.Danger, ex.Message);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostExtend()
    {
        var reservationResponse = await _abapi.client.Reservations.Extend(Entity.Id, Entity.ExpirationDate);
        if (reservationResponse.Data != null)
        {
            Entity = Entity.Map(reservationResponse.Data);
            var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
            BoxData = boxes.Data[0];

            SetFlash(FlashMessageType.Info, "Reservation extended");
        }
        else
        {
            var reservation = await _abapi.client.Reservations.Get(Entity.Id);
            Entity = Entity.Map(reservation);
            var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
            BoxData = boxes.Data[0];

            SetFlash(FlashMessageType.Danger, reservationResponse.Metadata);
        }

        Action = "Extend";
        return Page();
    }

    public async Task<IActionResult> OnGetCreateReservation(int boxId)
    {
        var boxes = await _abapi.client.Boxes.Get(boxId);
        BoxData = boxes.Data[0];

        Entity = new ReservationModel()
        {
            Id = Guid.NewGuid().ToString(),
            BoxId = BoxData.Id,
            ExpirationDate = DateTime.Now.AddHours(1)
        };

        SetFlash(FlashMessageType.Info, "Creating reservation");
        Action = "Create";
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var boxes = await _abapi.client.Boxes.Get(Entity.BoxId);
        BoxData = boxes.Data[0];
        
        var response = await _abapi.client.Reservations.Reserve(Entity.Id.ToString(), Entity.BoxId, Entity.PackageNumber,
            Entity.ExpirationDate,
            Entity.Depth, Entity.Height, Entity.Width);

        if (response.Data != null)
        {
            Entity = Entity.Map(response.Data);
            SetFlash(FlashMessageType.Info, "Reservation created");
        }
        else
        {
            Entity = new ReservationModel();
            SetFlash(FlashMessageType.Danger, response.Metadata);
        }

        return Page();
    }
}
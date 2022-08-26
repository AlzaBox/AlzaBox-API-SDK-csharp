using AlzaBox.API.WebExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlzaBox.API.WebExample.Pages;

public class Reservations : BasePageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ABAPIService _abapi;

    public List<AlzaBox.API.Models.Reservation> ReservationsData { get; set; }

    public SelectList StatusesSelectList;
    
    [BindProperty]
    public ReservationSearchModel ReservationSearch { get; set; }

        
    
    public Reservations(ILogger<IndexModel> logger, ABAPIService abapiService)
    {
        _logger = logger;
        _abapi = abapiService;
        _abapi.IsSignedIn(true);

        var reservationStatuses = new[]
        {
            new { ID = "Canceled", Name = "Canceled reservation" },
            new { ID = "Expired", Name = "Automatically canceled reservation after expiration" },
            new { ID = "Reserved", Name = "Valid reservation not yet stocked" },
            new { ID = "Incomplete", Name = "Partially stocked or package did not fit the box" },
            new { ID = "Stocking", Name = "Currently being filled into a box" },
            new { ID = "Stocked_Locked", Name = "Stocked, cannot be picked up by the customer" },
            new { ID = "Stocked", Name = "Stocked, ready to be picked up by the customer" },
            new { ID = "Returning", Name = "Will be picked up with the next driver login" },
            new { ID = "Returned", Name = "Picked up by the driver" },
            new { ID = "Picked_Up", Name = "Picked up by the customer" }
        };

        StatusesSelectList = new SelectList(reservationStatuses, "ID", "ID", 1);
    }

    public async Task<IActionResult> OnGet()
    {
        ReservationsData = new List<AlzaBox.API.Models.Reservation>();
        try
        {
            var reservations = await _abapi.client.Reservations.GetAll();
            ReservationsData.AddRange(reservations.Data);
        }
        catch (Exception ex)
        {
            if (ex.HResult == (int)StatusCodes.Status401Unauthorized)
            {
                return RedirectToPage("login");
            }
            else
            {
                SetFlash(FlashMessageType.Danger, ex.Source + ex.Message);
            }
        }
        
        
        return Page();
    }

    public async Task<IActionResult> OnPostSearch()
    {
        ReservationsData = new List<AlzaBox.API.Models.Reservation>();
        
        if (!string.IsNullOrWhiteSpace(ReservationSearch.Id))
        {
            var reservation = await _abapi.client.Reservations.Get(ReservationSearch.Id);
            if (reservation != null)
            {
                ReservationsData.Add(reservation);
            }

            return Page();
        }
        
        if (!string.IsNullOrWhiteSpace(ReservationSearch.Status))
        {
            try
            {
                var reservationsResponse = await _abapi.client.Reservations.GetAll(10, 0, ReservationSearch.Status);
                ReservationsData.AddRange(reservationsResponse.Data);
            }
            catch (Exception ex)
            {
                SetFlash(FlashMessageType.Danger, ex.Source);
            }
            
            return Page();
        }

        var allreservations = await _abapi.client.Reservations.GetAll();
        ReservationsData.AddRange(allreservations.Data);
        
        return Page();        
    }
}
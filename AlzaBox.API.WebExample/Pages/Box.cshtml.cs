using AlzaBox.API.Clients;
using AlzaBox.API.WebExample.Models;
using Microsoft.AspNetCore.Mvc;


namespace AlzaBox.API.WebExample.Pages;
public class Box : BasePageModel
{
    public AlzaBox.API.Models.Box? BoxData { get; set; }
    
    [BindProperty]
    public PackageSizeModel PackageSize { get; set; }
    public float? Occupancy { get; set; }
    
    private readonly ABAPIService _abapi;

    [BindProperty]
    public int BoxId { get; set; }

    public Box(ABAPIService abapiService)
    {
        _abapi = abapiService;
        _abapi.IsSignedIn(true);
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var boxes = await _abapi.client.Boxes.Get(id);
        BoxData = boxes.Data[0];
        BoxId = id;
        
        return Page();        
    }
    public async Task<IActionResult> OnGetWithOccupancy(int id)
    {
        var boxes = await _abapi.client.Boxes.Get(id);
        BoxData = boxes.Data[0];
        BoxId = id;
        Occupancy = await _abapi.client.Boxes.GetBoxOccupancy(id);
        
        return Page();
    }

    public async Task<IActionResult> OnPostPackage()
    {
        if (PackageSize.Width.HasValue && PackageSize.Height.HasValue && PackageSize.Depth.HasValue)
        {
            var boxResponse = await _abapi.client.Boxes.GetBoxFitting(PackageSize.Width.Value, PackageSize.Height.Value,
                PackageSize.Depth.Value, BoxId);
            BoxData = boxResponse.Data[0];
        }
        else
        {
            var boxes = await _abapi.client.Boxes.Get(BoxId);
            BoxData = boxes.Data[0];
        }
        
        return Page();
    }
}

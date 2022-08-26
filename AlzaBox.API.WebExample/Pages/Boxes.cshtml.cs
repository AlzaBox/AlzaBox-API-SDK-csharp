using System.Data;
using AlzaBox.API.WebExample.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;


namespace AlzaBox.API.WebExample.Pages;

public class Boxes : BasePageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ABAPIService _abapi;

    [BindProperty]
    public BoxSearchModel? BoxSearch { get; set; }
    public List<AlzaBox.API.Models.Box>? BoxesData { get; set; }
    
    public Boxes(ILogger<IndexModel> logger, ABAPIService abapiService)
    {
        _abapi = abapiService; 
        _abapi.IsSignedIn(true);
    }

    public async Task<IActionResult> OnGet()
    {
        BoxesData = new List<AlzaBox.API.Models.Box>();
        try
        {
            var boxes = await _abapi.client.Boxes.GetAll();
            BoxesData = boxes.Data;
        }
        catch (HttpRequestException ex)
        {
            SetFlash(FlashMessageType.Danger, ex.Source + ex.Message);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostSearch()
    {
        BoxesData = new List<AlzaBox.API.Models.Box>();
        
        if (!string.IsNullOrWhiteSpace(BoxSearch.Id))
        {
            var boxId = int.Parse(BoxSearch.Id);
            var boxes = await _abapi.client.Boxes.Get(boxId);
            BoxesData.AddRange(boxes.Data);
            return Page();
        }
        
        if (!string.IsNullOrWhiteSpace(BoxSearch.Name))
        {
            var boxes = await _abapi.client.Boxes.GetByName(BoxSearch.Name);
            BoxesData.AddRange(boxes.Data);
            return Page();
        }

        var allboxes = await _abapi.client.Boxes.GetAll();
        BoxesData.AddRange(allboxes.Data);
        
        return Page();
    }
    
    public async Task<FileResult> OnPostExport()
    {
        var allboxes = await _abapi.client.Boxes.GetAll();
        var boxesflat = new List<BoxFlatModel>();

        foreach (var box in allboxes.Data)
        {
             boxesflat.Add(new BoxFlatModel()
             {
                 Id = box.Id,
                 Name = box.Attributes.Name,
                 Lat = box.Attributes.Gps.Lat,
                 Lng = box.Attributes.Gps.Lng,
                 City = box.Attributes.Adress.City,
                 StreetWithNumber = box.Attributes.Adress.StreetWithNumber,
                 ZIP = box.Attributes.Adress.Zip,
                 Description = box.Attributes.Description,
                 CountryShortCode = box.Attributes.CountryShortCode
             });
        }
        
        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[9] 
        { new DataColumn("Id"),
            new DataColumn("Name"),
            new DataColumn("Description"),
            new DataColumn("StreetWithNumber"), 
            new DataColumn("City"),
            new DataColumn("ZIP"),
            new DataColumn("CountryShortCode"),
            new DataColumn("Lat"),            
            new DataColumn("Lng")
        });
        
        foreach (var box in boxesflat)
        {
            dt.Rows.Add(box.Id, box.Name, box.Description, box.StreetWithNumber, box.City, box.ZIP, box.CountryShortCode, box.Lat, box.Lng);
        }
 
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AlzaBoxes.xlsx");
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlzaBox.API.WebExample.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ABAPIService _abapi;

    public string Folder { get; set; }
    public List<AlzaBox.API.Models.Box>? Boxes { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        
        Folder = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        var fileName = "test.txt";
        var filePath = Path.Combine(Folder, fileName);

        System.IO.File.WriteAllText("test.txt", $"všechno funguje {DateTime.Now.ToShortTimeString()} ... {Folder}");
        
        return Page();
    }
}
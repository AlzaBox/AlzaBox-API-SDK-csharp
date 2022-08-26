using AlzaBox.API.WebExample.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlzaBox.API.WebExample.Pages.WebHooks;

public class ChangeStatus : PageModel
{
    private readonly ApplicationDbContext _db;
    public IEnumerable<ChangeStatusRequest> ChangeStatusRequests { get; set; }
    public ChangeStatus(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet()
    {
        ChangeStatusRequests = _db.ChangeStatusRequests.Select(x => x).OrderByDescending(x=>x.Id).ToList();
    }
}
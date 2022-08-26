using Microsoft.AspNetCore.Mvc;
using AlzaBox.API.Models;

namespace AlzaBox.API.WebExample.Pages;

public class Login : BasePageModel
{
    [BindProperty]
    public Credentials Credentials { get; set; }

    private readonly ABAPIService _abapi;

    public Login(ABAPIService abapi)
    {
        _abapi = abapi;
    }

    public async Task<IActionResult> OnPost()
    {
        _abapi.Authenticate(Credentials);
        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostLogout()
    {
        _abapi.Logout();
        return RedirectToPage("Login");
    }
}
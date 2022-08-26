using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AlzaBox.API.WebExample.Pages;

public enum FlashMessageType
{
    Primary,
    Secondary,
    Success,
    Danger,
    Warning,
    Info,
    Light,
    Dark,
}

public class BasePageModel : PageModel
{
    public FlashMessageType FlashType;
    public string? FlashText;

    public void SetFlash(FlashMessageType type, string text)
    {
        TempData["FlashMessage.Type"] = type;
        TempData["FlashMessage.Text"] = text;

        FlashType = type;
        FlashText = text;
    }
}
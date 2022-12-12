using AlzaBox.API.WebExample.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace ABBasicWebApp.Controllers;

[EnableCors]
[ApiController]
public class Webhooks : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly HttpContext _httpContext;
    
    public Webhooks(IHttpContextAccessor contextAccessor, ApplicationDbContext db)
    {
        _httpContext = contextAccessor.HttpContext;
        _db = db;
    }
    
    [HttpPost]
    [Route("/api/webhooks/change_state")]
    [Route("/api/webhooks/change_status")]
    public async Task<IActionResult> ChangeStatus()
    {
        _httpContext.Request.EnableBuffering();
        _httpContext.Request.Body.Position = 0;
        var bodyAsText = await new StreamReader(_httpContext.Request.Body).ReadToEndAsync();
        _httpContext.Request.Body.Position = 0;
        
        var changeStatus = new ChangeStatusRequest()
        {
            CreatedAt = DateTime.Now,
            RequestBody = bodyAsText,
            IP = _httpContext.Connection.RemoteIpAddress.ToString(),
            RequestHeader =_httpContext.Request.Headers.Authorization.ToString()
        };
        _db.Add(changeStatus);
        _db.SaveChanges();

        return new OkResult();
    }
}
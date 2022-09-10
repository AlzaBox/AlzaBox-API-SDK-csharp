using AlzaBox.API.Models;
using AlzaBox.API.Clients;

namespace AlzaBox.API.WebExample;

public class ABAPIService
{
    private readonly IConfiguration _configuration;
    private readonly Credentials _credentials;
    public readonly AlzaBoxClient client;
    private readonly HttpContext _httpContext;

    public ABAPIService(IHttpContextAccessor contextAccessor)
    {
        _httpContext = contextAccessor.HttpContext;
        client = new AlzaBoxClient();
        client.ExternalLogin(GetCookieToken());
    }

    public string? GetCookieToken()
    {
        return _httpContext.Request.Cookies["AccessToken"];
    }

    public bool IsSignedIn(bool redirectToLogin = false)
    {
        var accessToken = GetCookieToken();
        var signedIn = !string.IsNullOrWhiteSpace(accessToken);
        if (!signedIn & redirectToLogin)
        {
            _httpContext.Response.Redirect("login");
        }

        return signedIn;
    }

    public void Logout()
    {
        _httpContext.Response.Cookies.Delete("AccessToken");
    }
    
    public void Authenticate(Credentials? credentials = null)
    {
        if (credentials == null)
        {
            credentials = _credentials;
        }

        try
        {
            var authenticateTask = client.Login(credentials.UserName, credentials.Password,
                credentials.ClientId,
                credentials.ClientSecret);

            authenticateTask.Wait();
            _httpContext.Response.Cookies.Append("AccessToken", client.AccessToken);
        }
        catch (HttpRequestException ex)
        {
            _httpContext.Response.Redirect("login");
        }
    }
}
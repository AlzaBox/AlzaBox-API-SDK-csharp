using AlzaBox.API.Models;
using AlzaBox.API.Clients;

namespace AlzaBox.API.WebExample;

public class ABAPIService
{
    private readonly IConfiguration _configuration;
    private readonly Credentials _credentials;
    public readonly AlzaBoxClient client;
    public readonly AlzaBox.API.V2.Clients.AlzaBoxClient clientV2;
    private readonly HttpContext _httpContext;

    public ABAPIService(IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        var abIdmUrl = configuration["ABAPIService:ABIdmUrl"];
        var abConnectorUrl = configuration["ABAPIService:ABConnectorUrl"];
        var abBaseLockersUrl = configuration["ABAPIService: ABBaseLockerUrl"];
        
        _httpContext = contextAccessor.HttpContext;
        
        client = new AlzaBoxClient(abIdmUrl, abConnectorUrl);
        client.ExternalLogin(GetCookieToken());

        clientV2 = new AlzaBox.API.V2.Clients.AlzaBoxClient(abIdmUrl, abConnectorUrl, abBaseLockersUrl);
        clientV2.SetAccessToken(GetCookieToken());
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
using System.Net.Http.Headers;
using AlzaBox.API.Models;

namespace AlzaBox.API.Clients;

public class AlzaBoxClient
{
    private readonly string _alzaBoxIdmUrl;
    private readonly string _alzaBoxLockerUrl;
    private readonly HttpClient _restABClient;
    private readonly AuthenticationClient _authenticationClient;

    public string AccessToken { get; set; }
    public BoxClient Boxes { get; set; }
    public ReservationClient Reservations { get; set; }
    
    
    public AlzaBoxClient(string? abIdmUrl = Constants.TestIdentityBaseUrl, string? abConnectorUrl = Constants.TestParcelLockersBaseUrl)
    {
        _authenticationClient = new AuthenticationClient(abIdmUrl);
        _restABClient = new HttpClient();
        _restABClient.BaseAddress = new Uri(abConnectorUrl);
        _restABClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _restABClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
        {
            NoCache = true
        };
        
        Boxes = new BoxClient(_restABClient);
        Reservations = new ReservationClient(_restABClient);
        
    }

    public async Task<AuthenticationResponse> Login(string username, string password, string clientId,
        string clientSecret)
    {
        var credentials = new Credentials()
        {
            UserName = username,
            Password = password,
            ClientId = clientId,
            ClientSecret = clientSecret
        };
        
        var authenticationResponse = await _authenticationClient.Authenticate(credentials);
        AccessToken = authenticationResponse.AccessToken;
        _restABClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        
        return authenticationResponse;
    }

    public async void ExternalLogin(string accessToken)
    {
        AccessToken = accessToken;
        _restABClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);        
    }
}
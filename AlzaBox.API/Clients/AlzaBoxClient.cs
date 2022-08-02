using AlzaBox.API.Models;
using RestSharp;

namespace AlzaBox.API.Clients;

public class AlzaBoxClient
{
    private readonly string _alzaBoxIdmUrl;
    private readonly string _alzaBoxLockerUrl;
    private readonly RestClient _restABClient;
    private readonly AuthenticationClient _authenticationClient;

    private string AccessToken { get; set; }
    public BoxClient Boxes { get; set; }
    public ReservationClient Reservations { get; set; }
    
    
    public AlzaBoxClient(string? alzaBoxIDMUrl = null, string? alzaBoxLockerUrl = null)
    {
        alzaBoxIDMUrl ??= Constants.TestIdentityBaseUrl;
        alzaBoxLockerUrl ??= Constants.TestParcelLockersBaseUrl;
        
        _authenticationClient = new AuthenticationClient(alzaBoxIDMUrl);
        _restABClient = new RestClient(alzaBoxLockerUrl);
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
        return authenticationResponse;
    }
}
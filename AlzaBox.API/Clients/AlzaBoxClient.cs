using AlzaBox.API.Models;
using RestSharp;

namespace AlzaBox.API.Clients;

public class AlzaBoxClient
{
    private readonly string _alzaBoxIdmUrl;
    private readonly string _alzaBoxLockerUrl;
    private readonly RestClient _restAuthClient;
    private readonly RestClient _restABClient;
    private readonly AuthenticationClient _authenticationClient;

    private string AccessToken { get; set; }

    public BoxClient Boxes { get; set; }
    public ReservationClient Reservations { get; set; }
    
    
    public AlzaBoxClient(string alzaBoxIDMUrl, string alzaBoxLockerUrl)
    {
        _alzaBoxIdmUrl = alzaBoxIDMUrl;
        _alzaBoxLockerUrl = alzaBoxLockerUrl;
        _restAuthClient = new RestClient(_alzaBoxIdmUrl);
        _restABClient = new RestClient(_alzaBoxLockerUrl);
        _authenticationClient = new AuthenticationClient(alzaBoxIDMUrl);
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
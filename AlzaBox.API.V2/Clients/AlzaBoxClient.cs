using System.Net.Http.Headers;
using AlzaBox.API.V2.Models;

namespace AlzaBox.API.V2.Clients;

public class AlzaBoxClient
{
    private readonly string _alzaBoxIdmUrl;
    private readonly string _alzaBoxLockerUrl;
    private readonly HttpClient _restABClient;
    private readonly HttpClient _restABBaseClient;
    private readonly AuthenticationClient _authenticationClient;

    private Credentials Credentials { get; set; }

    public string AccessToken { get; set; }
    public BoxClient Boxes { get; set; }
    public ReservationClient Reservations { get; set; }
    public CourierClient Couriers  { get; set; }
    public VirtualBoxClient VirtualBox { get; set; }

    public AlzaBoxClient(string? abIdmUrl = Constants.TestIdentityBaseUrl, string? abConnectorUrl = Constants.TestParcelLockersBaseUrl, string? abBaseLockersUrl = Constants.TestVirtualLockersUrl)
    {
        abIdmUrl = (string.IsNullOrWhiteSpace(abIdmUrl)) ? Constants.TestIdentityBaseUrl : abIdmUrl;
        abConnectorUrl = (string.IsNullOrWhiteSpace(abConnectorUrl)) ? Constants.TestParcelLockersBaseUrl : abConnectorUrl;

        _authenticationClient = new AuthenticationClient(abIdmUrl);
        _restABClient = new HttpClient();
        _restABClient.BaseAddress = new Uri(abConnectorUrl);
        _restABClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _restABClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
        {
            NoCache = true
        };
        
        _restABBaseClient = new HttpClient();
        //_restABBaseClient.BaseAddress = new Uri(abBaseLockersUrl);
        _restABBaseClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _restABBaseClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
        {
            NoCache = true
        };
        
        Boxes = new BoxClient(_restABClient);
        Reservations = new ReservationClient(_restABClient);
        Couriers = new CourierClient(_restABClient);
        VirtualBox = new VirtualBoxClient(_restABBaseClient);
    }

    public async Task<AuthenticationResponse> Login(string username, string password, string clientId,
        string clientSecret)
    {
        Credentials = new Credentials()
        {
            UserName = username,
            Password = password,
            ClientId = clientId,
            ClientSecret = clientSecret
        };
        
        var authenticationResponse = await _authenticationClient.Authenticate(Credentials);
        AccessToken = authenticationResponse.AccessToken;
        _restABClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        _restABBaseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        
        return authenticationResponse;
    }

    public async void SetAccessToken(string accessToken)
    {
        AccessToken = accessToken;
        _restABClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        _restABBaseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
    }
}
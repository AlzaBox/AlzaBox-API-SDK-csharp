using System.Net.Http.Headers;
using System.Text.Json;
using AlzaBox.API.Models;

namespace AlzaBox.API.Clients;

public class AuthenticationClient
{
    private readonly HttpClient _httpClient;

    public AuthenticationClient(string identityEnvironment)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(identityEnvironment);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<AuthenticationResponse> Authenticate(Credentials credentials)
    {
        var body = new List<KeyValuePair<string, string>>();
        body.Add(new KeyValuePair<string, string>("username", credentials.UserName));
        body.Add(new KeyValuePair<string, string>("password", credentials.Password));
        body.Add(new KeyValuePair<string, string>("client_id", credentials.ClientId));
        body.Add(new KeyValuePair<string, string>("client_secret", credentials.ClientSecret));

        body.Add(new KeyValuePair<string, string>("scope", Constants.ScopeKonzoleAccess));
        body.Add(new KeyValuePair<string, string>("grant_type", Constants.GrantTypePassword));

        var httpContent = new FormUrlEncodedContent(body);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.ContentTypeFormUrlUncoded);

        var res = await _httpClient.PostAsync("", httpContent);

        if (res.IsSuccessStatusCode)
        {
            var content = await res.Content.ReadAsStringAsync();
            var authResponse =
                JsonSerializer.Deserialize<AuthenticationResponse>(content);
            return authResponse;
        }
        else
        {
            var content = await res.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, res.StatusCode);
        }
    }
}
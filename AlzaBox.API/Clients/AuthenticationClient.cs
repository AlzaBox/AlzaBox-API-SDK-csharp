using System.Data;
using RestSharp;
using RestSharp.Serializers;
using AlzaBox.API.Models;

namespace AlzaBox.API.Clients;

public class AuthenticationClient
{
    private readonly RestClient _restClient;

    public AuthenticationClient(string identityEnvironment)
    {
        _restClient = new RestClient(identityEnvironment);
    }
    
    public async Task<AuthenticationResponse> Authenticate(Credentials credentials)
    {
        var authRequest = GetAuthRequest(credentials);
        
        try
        {
            var response = await _restClient.PostAsync<AuthenticationResponse>(authRequest);
            return response;
        }
        catch (Exception ex)
        {
            var response = new AuthenticationResponse()
            {
                ExpiresIn = 0,
                AccessToken = "",
                TokenType = "INVALID"
            };
            return response;
        }
    }
    
    private RestRequest GetAuthRequest(Credentials credentials)
    {
        var request = new RestRequest();

        request.AddHeader("Content-Type", Constants.ContentTypeFormUrlUncoded);
        request.AddHeader("Accept", ContentType.Json);
            
        request.AddParameter("username", credentials.UserName, ParameterType.GetOrPost);
        request.AddParameter("password", credentials.Password, ParameterType.GetOrPost);
        request.AddParameter("client_id", credentials.ClientId, ParameterType.GetOrPost);
        request.AddParameter("client_secret", credentials.ClientSecret, ParameterType.GetOrPost);
            
        request.AddParameter("scope", Constants.ScopeKonzoleAccess, ParameterType.GetOrPost);
        request.AddParameter("grant_type", Constants.GrantTypePassword, ParameterType.GetOrPost);

        return request;
    }    
}
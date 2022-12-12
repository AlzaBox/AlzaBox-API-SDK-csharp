using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace AlzaBox.API.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> GetWithQueryStringAsync(this HttpClient httpClient, string relativeUri, 
        QueryString queryString)
    {
        var uri = new Uri(httpClient.BaseAddress, relativeUri);
        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Query = queryString.ToUriComponent();
        return await httpClient.GetAsync(uriBuilder.Uri);
    }
    
    public static async Task<T> GetWithQueryStringAsync<T>(this HttpClient httpClient, string relativeUri,
        QueryString queryString)
    {
        var response = await httpClient.GetWithQueryStringAsync(relativeUri, queryString);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonSerializer.Deserialize<T>(content);
            return deserializedResponse;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, response.StatusCode);
        }        
    }
    
    public static async Task<HttpResponseMessage> SendJsonAsync(this HttpClient httpClient, HttpMethod method, string? requestUri,
        string serializedContent)
    {
        var request = new HttpRequestMessage(method, requestUri);
        request.Content = new StringContent(serializedContent, Encoding.UTF8);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await httpClient.SendAsync(request);
        return response;
    }
    
    public static async Task<TO> SendJsonAsync<TO,TI>(this HttpClient httpClient, HttpMethod method, string? uri, TI deserializedContent)
    {
        var serializedContent = JsonSerializer.Serialize(deserializedContent);
        var response = await httpClient.SendJsonAsync(method, uri, serializedContent);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonSerializer.Deserialize<TO>(content);
            return deserializedResponse;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, response.StatusCode);
        }        
    }    
}
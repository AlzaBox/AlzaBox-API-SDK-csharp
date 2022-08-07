using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace AlzaBox.API.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> GetWithQueryStringAsync(this HttpClient httpClient, string uri, 
        Dictionary<string, string> queryStringParams)
    {
        var url = QueryHelpers.AddQueryString(uri, queryStringParams);
        return await httpClient.GetAsync(url);
    }

    public static async Task<T> GetWithQueryStringAsync<T>(this HttpClient httpClient, string uri,
        Dictionary<string, string> queryStringParams)
    {
        var response = await httpClient.GetWithQueryStringAsync(uri, queryStringParams);
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
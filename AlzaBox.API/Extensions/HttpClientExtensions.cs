using Microsoft.AspNetCore.WebUtilities;

namespace AlzaBox.API.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> GetWithQueryStringAsync(this HttpClient client, string uri, 
        Dictionary<string, string> queryStringParams)
    {
        var url = QueryHelpers.AddQueryString(uri, queryStringParams);

        return await client.GetAsync(url);
    }
}
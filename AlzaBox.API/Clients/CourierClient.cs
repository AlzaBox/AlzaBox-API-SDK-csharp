using System.Runtime.Serialization;
using System.Text.Json;
using AlzaBox.API.Extensions;
using Microsoft.AspNetCore.Http;

namespace AlzaBox.API.Clients;

using AlzaBox.API.Models;

public class CourierClient
{
    private readonly HttpClient _httpClient;

    public CourierClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<GetCouriersResponse> Get(string login = "")
    {
        return await GetBase(login);
    }

    private async Task<AlzaBox.API.Models.GetCouriersResponse> GetBase(string login = "", int pageLimit = 10,
        int pageOffset = 0,
        string status = "")
    {
        var queryString = new QueryString();
        queryString = queryString.Add("fields[courier]", "boxes");
        
        if (!string.IsNullOrWhiteSpace(login))
        {
            queryString = queryString.Add("filter[login]", login);
            var courierResponse = await _httpClient.GetWithQueryStringAsync<GetCourierResponse>("v2/courier", queryString);
            var couriers = new List<CourierWithAttributes>();
            couriers.Add(courierResponse.Data.Courier);

            var response = new GetCouriersResponse();
            response.Data = new CourierDataWithManyCourierItems()
            {
                Couriers = couriers
            };

            return response;
        }
        else
        {
            queryString = queryString.Add("page[limit]", pageLimit.ToString());
            queryString = queryString.Add("page[offset]", pageOffset.ToString());
            return await _httpClient.GetWithQueryStringAsync<GetCouriersResponse>("v2/couriers", queryString);
        }
    }

    public async Task<CreateOrUpdateCourierResponse> Create(string login, string pin,
        List<CourierBox> boxes)
    {
        return await CreateOrUpdate(HttpMethod.Post, login, pin, boxes);
    }

    public async Task<CreateOrUpdateCourierResponse> Update(string login, string pin,
        List<CourierBox> boxes)
    {
        return await CreateOrUpdate(HttpMethod.Patch, login, pin, boxes);
    }
    
    private async Task<CreateOrUpdateCourierResponse> CreateOrUpdate(HttpMethod method, string login, string pin, List<CourierBox> boxes)
    {
        var content = new CreateOrUpdateCourierRequest()
        {
            Data = new CourierDataWithOneCourierItemWithoutAttributes()
            {
                Courier = new CourierWithoutAttributes()
                {
                    Login = login,
                    Pin = pin,
                    Boxes = boxes
                }
            } 
        };

        var serializedContent = JsonSerializer.Serialize(content);
        var response = await _httpClient.SendJsonAsync(method, "v2/courier?fields[courier]=boxes", serializedContent);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                var deserializedResponse = JsonSerializer.Deserialize<CreateOrUpdateCourierResponse>(responseContent);
                return deserializedResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
                throw new SerializationException("API Response dont match expected structure.");
            }
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(responseContent, null, response.StatusCode);
        }
    }
}
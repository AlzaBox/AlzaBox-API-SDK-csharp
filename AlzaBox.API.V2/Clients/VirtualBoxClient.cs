using System.Text.Json;
using AlzaBox.API.Extensions;
using AlzaBox.API.V2.Models;
using Microsoft.AspNetCore.Http;

namespace AlzaBox.API.V2.Clients
{
    public class VirtualBoxClient
    {
        private readonly HttpClient _httpClient;

        public VirtualBoxClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<VirtualBoxResponse> Stocked(string reservationId)
        {
            var response = await this.PatchVirtualBox(reservationId, new VirtualBoxRequest() { state = "STOCKED" });
            return response;
        }

        public async Task<VirtualBoxResponse> PickedUp(string reservationId)
        {
            var response = await this.PatchVirtualBox(reservationId, new VirtualBoxRequest() { state = "PICKED_UP" });
            return response;
        }

        private async Task<VirtualBoxResponse> PatchVirtualBox(string reservationId, VirtualBoxRequest virtualBoxRequest)
        {
            var query = new QueryString();
            query = query.Add("reservationId", reservationId);

            var relativePath = "virtualbox/v1/reservation";

            var baseUri = new Uri(Constants.TestVirtualLockersUrl);
            var uri = new Uri(baseUri, relativePath);
            var uriBuilder = new UriBuilder(uri);
            
            uriBuilder.Query = query.ToUriComponent();

            var serializedContent = JsonSerializer.Serialize(virtualBoxRequest);
            var httpResponseMessage =  await  _httpClient.SendJsonAsync(HttpMethod.Patch, uriBuilder.Uri.AbsoluteUri, serializedContent);
            var stringResponse = await httpResponseMessage.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<VirtualBoxResponse>(stringResponse);
            
            return deserializedResponse;

            //return await _httpClient.SendJsonAsync<VirtualBoxResponse, VirtualBoxRequest>(HttpMethod.Patch,
            //    uriBuilder.Uri.AbsoluteUri, virtualBoxRequest);
        }
    }
}
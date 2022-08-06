using System.Net;
using System.Text.Json;
using AlzaBox.API.Extensions;
using AlzaBox.API.Models;
using RestSharp;

namespace AlzaBox.API.Clients
{
    public class BoxClient
    {
        private readonly HttpClient _client;

        public BoxClient(HttpClient client)
        {
            _client = client;
        }
        
        public async Task<BoxesResponse> Get(int boxId)
        {
            var response = await this.GetBoxBase(boxId);
            return response;
        }

        public async Task<BoxesResponse> GetAll()
        {
            var response = await this.GetBoxBase(null, null, null, null, false, false);
            return response;
        }

        public async Task<float> GetBoxOccupancy(int boxId)
        {
            var boxResponse = await GetBoxBase(boxId, null, null, null, false, true);
            var occupancy = boxResponse.Data[0].Attributes.Occupancy;
            return occupancy.Value;
        }

        public async Task<BoxesResponse> GetBoxFitting(double packageWidth, double packageHeight, double packageDepth, int? boxId = null)
        {
            var boxResponse = await GetBoxBase(boxId, packageWidth, packageHeight, packageDepth, true, false);
            return boxResponse;
        }

        public async Task<BoxesResponse> GetByName(string name, bool full = false, bool occupancy = false)
        {
            var boxContent = new BoxesResponse();
            var searched = new List<Box>();
            var allboxes = await GetBoxBase(null, null, null, null, full, occupancy);
            foreach (var box in allboxes.Data)
            {
                if (box.Attributes.Name.Contains(name))
                {
                    searched.Add(box);
                }
            }

            boxContent.Data = searched;
            return boxContent;
        }
        
        private async Task<BoxesResponse> GetBoxBase(int? boxId = null, double? packageWidth = null,
            double? packageHeight = null, double? packageDepth = null, bool full = false, bool occupancy = false)
        {
            var query = new Dictionary<string, string>()
            {
                ["page[limit]"] = pageLimit.ToString(),
                ["page[Offset]"] = pageOffset.ToString(),
            };


            if (packageDepth.HasValue)
            {
                //boxRequest.AddParameter("filter[package][depth]", packageDepth.Value);
                query.Add("filter[package][depth]", packageDepth.Value.ToString());
            }

            if (packageHeight.HasValue)
            {
                //boxRequest.AddParameter("filter[package][height]", packageHeight.Value);
                query.Add("filter[package][height]", packageHeight.Value.ToString());
            }
            
            if (packageWidth.HasValue)
            {
                //boxRequest.AddParameter("filter[package][width]", packageWidth.Value);
                query.Add("filter[package][width]", packageWidth.Value.ToString());
            }
            
            if ((boxId.HasValue) && (boxId > 0))
            {
                //boxRequest.AddParameter("filter[id]", boxId.Value);
                query.Add("filter[id]", boxId.Value.ToString());
            }

            if ((boxId > 0) && full)
            {
                query.Add("fields[box]", "fittingPackages");
                query.Add("fields[box]", "unavaiableReason");
                query.Add("fields[box]", "tooLargePackages");
                query.Add("fields[box]", "requiredSlots");
            }

            if (occupancy)
            {
                query.Add("fields[box]", "occupancy");
            }

            var response = await _client.GetWithQueryStringAsync("box", query);
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception()
                {
                    HResult = (int)response.StatusCode,
                    Source = response.Content
                };
            } else if  (response.StatusCode == HttpStatusCode.NotFound)
            {
                var boxes = new BoxesResponse()
                {
                    Data = new List<Box>()
                };
                return boxes;
            } else if (response.IsSuccessful)
            {
                var boxes = JsonSerializer.Deserialize<BoxesResponse>(response.Content);
                return boxes;
            }
            else
            {
                throw new Exception()
                {
                    HResult = (int)response.StatusCode,
                    Source = response.Content
                };
            }
        }      
        
        private async Task<RestRequest> CreateBaseBoxRequest()
        {
            var boxRequest = new RestRequest();
            boxRequest.Resource = "box";
            boxRequest.AddHeader("Cache-Control", "no-cache");
            boxRequest.AddHeader("Authorization", $"Bearer {this.accessToken}");
            boxRequest.AlwaysMultipartFormData = false;

            boxRequest.AddParameter("fields[box]", "deliveryPin");
            boxRequest.AddParameter("fields[box]", "name");
            boxRequest.AddParameter("fields[box]", "address");
            boxRequest.AddParameter("fields[box]", "gps");
            boxRequest.AddParameter("fields[box]", "description");
            boxRequest.AddParameter("fields[box]", "openingHours");
            boxRequest.AddParameter("fields[box]", "slots");
            boxRequest.AddParameter("fields[box]", "countryShortCode");

            return boxRequest;
        }        
    }
}
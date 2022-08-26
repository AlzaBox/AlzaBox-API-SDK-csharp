using AlzaBox.API.Extensions;
using AlzaBox.API.Models;
using Microsoft.AspNetCore.Http;

namespace AlzaBox.API.Clients
{
    public class BoxClient
    {
        private readonly HttpClient _httpClient;

        public BoxClient(HttpClient httpClient) => _httpClient = httpClient;
        
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
            var allBoxes = await GetBoxBase(null, null, null, null, full, occupancy);
            foreach (var box in allBoxes.Data)
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
            var query = new QueryString();
            query = query.Add("fields[box]", "deliveryPin");
            query = query.Add("fields[box]", "name");
            query = query.Add("fields[box]", "address");
            query = query.Add("fields[box]", "gps");
            query = query.Add("fields[box]", "description");
            query = query.Add("fields[box]", "openingHours");
            query = query.Add("fields[box]", "slots");
            query = query.Add("fields[box]", "countryShortCode");

            if (packageDepth.HasValue)
            {
                query = query.Add("filter[package][depth]", packageDepth.Value.ToString());
            }

            if (packageHeight.HasValue)
            {
                query = query.Add("filter[package][height]", packageHeight.Value.ToString());
            }
            
            if (packageWidth.HasValue)
            {
                query = query.Add("filter[package][width]", packageWidth.Value.ToString());
            }
            
            if ((boxId.HasValue) && (boxId > 0))
            {
                query = query.Add("filter[id]", boxId.Value.ToString());
            }

            if ((boxId > 0) && full)
            {
                query = query.Add("fields[box]", "fittingPackages");
                query = query.Add("fields[box]", "unavailableReason");
                query = query.Add("fields[box]", "tooLargePackages");
                query = query.Add("fields[box]", "requiredSlots");
            }

            if (occupancy)
            {
                query = query.Add("fields[box]", "occupancy");
            }

            return await _httpClient.GetWithQueryStringAsync<BoxesResponse>("box", query);
        }
    }
}
using AlzaBox.API.Extensions;
using AlzaBox.API.V2.Models;
using Microsoft.AspNetCore.Http;

namespace AlzaBox.API.V2.Clients
{
    public class BoxClient
    {
        private readonly HttpClient _httpClient;

        public BoxClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<BoxResponse> Get(int boxId, bool photos = false)
        {
            var response = await this.GetBoxBase<BoxResponse>(boxId, photos: photos);
            return response;
        }

        public async Task<BoxesResponse> GetAll(bool photos = false)
        {
            var response = await this.GetBoxBase<BoxesResponse>(null, null, null, null, false, false, photos: photos);
            return response;
        }

        public async Task<float> GetBoxOccupancy(int boxId)
        {
            var boxResponse = await GetBoxBase<BoxResponse>(boxId, null, null, null, false, true);
            var occupancy = boxResponse.Data.Box.Attributes.Occupancy;
            return occupancy.Value;
        }
        
        public async Task<BoxesResponse> GetBoxFitting(double packageWidth, double packageHeight, double packageDepth)
        {
            throw new Exception("Box fitting is deprecated API function");
            
            var boxResponse = await GetBoxBase<BoxesResponse>(null, packageWidth, packageHeight, packageDepth, true, false);
            return boxResponse;
        }

        public async Task<BoxesResponse> GetByName(string name, bool full = false, bool occupancy = false, bool photos = false)
        {
            var boxContent = new BoxesResponse();
            var searched = new List<Box>();
            var allBoxes = await GetBoxBase<BoxesResponse>(null, null, null, null, full, occupancy, photos);
            foreach (var box in allBoxes.Data.Boxes)
            {
                if (box.Attributes.Name.Contains(name))
                {
                    searched.Add(box);
                }
            }

            boxContent.Data.Boxes = searched;
            return boxContent;
        }
        
        private async Task<T> GetBoxBase<T>(int? boxId = null, double? packageWidth = null,
            double? packageHeight = null, double? packageDepth = null, bool full = false, bool occupancy = false, bool photos = false)
        {
            var query = new QueryString();
            var relativeUri = "v2/boxes";
            
            if ((boxId.HasValue) && (boxId > 0))
            {
                query = query.Add("filter[id]", boxId.Value.ToString());
                relativeUri = "v2/box";
            }
            
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
            
            if ((boxId > 0) && full)
            {
                query = query.Add("fields[box]", "fittingPackages");
                query = query.Add("fields[box]", "unavailableReason");
                query = query.Add("fields[box]", "tooLargePackages");
                query = query.Add("fields[box]", "requiredSlots");
            }

            if (photos)
            {
                query = query.Add("fields[box]", "photos");
            }

            if (occupancy)
            {
                query = query.Add("fields[box]", "occupancy");
            }

            return await _httpClient.GetWithQueryStringAsync<T>(relativeUri, query);
        }
    }
}
using AlzaBox.API.Extensions;
using AlzaBox.API.Models;

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
            var query = new Dictionary<string, string>()
            {
                ["fields[box]"] = "deliveryPin",
                ["fields[box]"] = "name",
                ["fields[box]"] = "address",
                ["fields[box]"] = "gps",
                ["fields[box]"] = "description",
                ["fields[box]"] = "openingHours",                
                ["fields[box]"] = "slots",
                ["fields[box]"] = "countryShortCode"
            };

            if (packageDepth.HasValue)
            {
                query.Add("filter[package][depth]", packageDepth.Value.ToString());
            }

            if (packageHeight.HasValue)
            {
                query.Add("filter[package][height]", packageHeight.Value.ToString());
            }
            
            if (packageWidth.HasValue)
            {
                query.Add("filter[package][width]", packageWidth.Value.ToString());
            }
            
            if ((boxId.HasValue) && (boxId > 0))
            {
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

            return await _httpClient.GetWithQueryStringAsync<BoxesResponse>("box", query);
        }
    }
}
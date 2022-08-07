using AlzaBox.API.Extensions;
using AlzaBox.API.Models;

namespace AlzaBox.API.Clients;

public class ReservationClient
{
    private readonly HttpClient _httpClient;
    
    public ReservationClient(HttpClient httpClient) => _httpClient = httpClient;
    
    public async Task<AlzaBox.API.Models.Reservation> Get(string reservationId)
    {
        var response = await GetBase(reservationId);
        if (response.Data != null)
        {
            return response.Data[0];
        }
        else
        {
            return null;
        }
    }

    public async Task<AlzaBox.API.Models.ReservationsResponse> GetAll(int pageLimit = 10, int pageOffset = 0,
        string status = "")
    {
        var response = await GetBase("", pageLimit, pageOffset, status);
        return response;
    }

    private async Task<AlzaBox.API.Models.ReservationsResponse> GetBase(string reservationId = "", int pageLimit = 10,
        int pageOffset = 0,
        string status = "")
    {
        var query = new Dictionary<string, string>()
        {
            ["page[limit]"] = pageLimit.ToString(),
            ["page[Offset]"] = pageOffset.ToString(),
        };
        
        if (!string.IsNullOrWhiteSpace(reservationId))
        {
            query.Add("filter[Id]", reservationId);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query.Add("filter[Status]", status);
        }
        
        return await _httpClient.GetWithQueryStringAsync<ReservationsResponse>("reservation", query);
    }
    
    public async Task<ReservationsResponse> GetReservationStatus(string reservationId)
    {
        return await GetBase(reservationId);
    }    

    public async Task<ReservationResponse> Reserve(string id, int boxId, string packageNumber, int hoursFromNow)
    {
        var expirationDate = DateTime.Now.AddHours(hoursFromNow);
        var reservationResponse = await Reserve(id, boxId, packageNumber, expirationDate);
        return reservationResponse;
    }

    public async Task<ReservationResponse> Reserve(string id, int boxId, string packageNumber, DateTime expirationDate,
        float? depth = 1, float? height = 1, float? width = 1)
    {
        var expirationDateUtcString = expirationDate.ToString("O");
        var packages = new List<ReservationRequestPackages>();
        packages.Add(new ReservationRequestPackages()
        {
            BarCode = packageNumber,
            Depth = depth,
            Height = height,
            Width = width
        });

        var reservationRequestBody = new ReservationRequest()
        {
            Data = new ReservationRequestData()
            {
                Reservation = new Reservation()
                {
                    Id = id,
                    Attributes = new ReservationRequestAttributes()
                    {
                        Packages = packages,
                        ExpirationDate = expirationDateUtcString,
                        Type = "NON_BINDING"
                    },
                    Relationships = new ReservationRequestRelationships()
                    {
                        Box = new ReservationRequestBox()
                        {
                            Id = boxId
                        }
                    }
                }
            }
        };
        
        var response = await _httpClient.SendJsonAsync<ReservationResponse, ReservationRequest>(HttpMethod.Post, "reservation", reservationRequestBody);
        return response;
    }   

    public async Task<ReservationResponse> Extend(string reservationId, int hoursFromNow = 24)
    {
        var expirationDate = DateTime.Now.AddHours(hoursFromNow);
        var reservationResponse = await Extend(reservationId, expirationDate);
        return reservationResponse;
    }

    public async Task<ReservationResponse> Extend(string reservationId, DateTime expirationDate)
    {
        var expirationDateUtcString = expirationDate.ToString("O");
        var reservationRequestBody = new ReservationRequest()
        {
            Data = new ReservationRequestData()
            {
                Reservation = new Reservation()
                {
                    Id = reservationId,
                    Attributes = new ReservationRequestAttributes()
                    {
                        ExpirationDate = expirationDateUtcString,
                    },
                }
            }
        };

        return await PatchReservation(reservationRequestBody);
    }

    public async Task<ReservationResponse> Cancel(string reservationId)
    {
        var reservationRequestBody = new ReservationRequest()
        {
            Data = new ReservationRequestData()
            {
                Reservation = new Reservation()
                {
                    Id = reservationId,
                    Attributes = new ReservationRequestAttributes()
                    {
                        Status = "CANCELED"
                    },
                }
            }
        };

        return await PatchReservation(reservationRequestBody);
    }

    private async Task<ReservationResponse> PatchReservation(ReservationRequest reservationRequest)
    {
        return await _httpClient.SendJsonAsync<ReservationResponse, ReservationRequest>(HttpMethod.Patch, "reservation", reservationRequest);        
    }
}
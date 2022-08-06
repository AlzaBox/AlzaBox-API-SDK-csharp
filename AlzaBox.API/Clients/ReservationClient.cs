using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AlzaBox.API.Extensions;
using AlzaBox.API.Models;
using RestSharp;

namespace AlzaBox.API.Clients;

public class ReservationClient
{
    private readonly HttpClient _client;
    private readonly string _accessToken;

    public ReservationClient(HttpClient client, string? accessToken = null)
    {
        _client = client;
        _accessToken = accessToken;
    }


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
        
        var response = await _client.GetWithQueryStringAsync("reservation", query);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var reservationsResponse = JsonSerializer.Deserialize<ReservationsResponse>(content);
            return reservationsResponse;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, response.StatusCode);
        }
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

        var serializedDoc = JsonSerializer.Serialize(reservationRequestBody);
        var request = new HttpRequestMessage(HttpMethod.Post, "reservation");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = new StringContent(serializedDoc, Encoding.UTF8);
        var response = await _client.SendAsync(request);

        ReservationResponse reservationResponse;
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            reservationResponse = JsonSerializer.Deserialize<ReservationResponse>(content);
            return reservationResponse;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, response.StatusCode);
        }
    }

    public async Task<ReservationResponse> ExtendReservation(string reservationId, int hoursFromNow = 24)
    {
        var expirationDate = DateTime.Now.AddHours(hoursFromNow);
        var reservationResponse = await ExtendReservation(reservationId, expirationDate);
        return reservationResponse;
    }

    public async Task<ReservationResponse> ExtendReservation(string reservationId, DateTime expirationDate)
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

        var serializedDoc = JsonSerializer.Serialize(reservationRequestBody);
        var request = new HttpRequestMessage(HttpMethod.Patch, "reservation");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = new StringContent(serializedDoc, Encoding.UTF8);
        var response = await _client.SendAsync(request);

        ReservationResponse reservationResponse;
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            reservationResponse = JsonSerializer.Deserialize<ReservationResponse>(content);
            return reservationResponse;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, response.StatusCode);
        }
    }

    public async Task<ReservationResponse> CancelReservation(string reservationId)
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

        var serializedDoc = JsonSerializer.Serialize(reservationRequestBody);
        var request = new HttpRequestMessage(HttpMethod.Patch, "reservation");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = new StringContent(serializedDoc, Encoding.UTF8);
        var response = await _client.SendAsync(request);

        ReservationResponse reservationResponse;
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            reservationResponse = JsonSerializer.Deserialize<ReservationResponse>(content);
            return reservationResponse;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, response.StatusCode);
        }
    }
}
using System.Net;
using System.Text.Json;
using AlzaBox.API.Models;
using RestSharp;

namespace AlzaBox.API.Clients;

public class ReservationClient
{
    private readonly RestClient _client;
    private readonly string _accessToken;

    public ReservationClient(RestClient client, string? accessToken = null)
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

    public async Task<AlzaBox.API.Models.ReservationsResponse> GetAll(int pageLimit = 10, int pageOffset = 0, string status = "")
    {
        var response = await GetBase("", pageLimit, pageOffset, status);
        return response;
    }

    private async Task<AlzaBox.API.Models.ReservationsResponse> GetBase(string reservationId = "", int pageLimit = 10, int pageOffset = 0,
        string status = "")
    {
        var reservationRequest = new RestRequest();
        reservationRequest.Resource = "reservation";
        reservationRequest.AddHeader("Cache-Control", "no-cache");
        reservationRequest.AddHeader("Authorization", $"Bearer {_accessToken}");
        reservationRequest.AlwaysMultipartFormData = false;
        reservationRequest.AddParameter("page[limit]", pageLimit);
        reservationRequest.AddParameter("page[Offset]", pageOffset);
        if (!string.IsNullOrWhiteSpace(reservationId))
        {
            reservationRequest.AddParameter("filter[Id]", reservationId);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservationRequest.AddParameter("filter[Status]", status);
        }

        var response = await _client.ExecuteAsync(reservationRequest, Method.Get);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new Exception()
            {
                HResult = (int)response.StatusCode,
                Source = response.Content
            };                
        }        

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var reservations = new ReservationsResponse();
            return reservations;
        }
        
        if (response.IsSuccessful)
        {
            var reservations = JsonSerializer.Deserialize<ReservationsResponse>(response.Content);
            return reservations;
        }
        else
        {
            var ex = new Exception()
            {
                Source = response.Content,
                HResult = (int)response.StatusCode
            };

            throw ex;
        }
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

        var reservationRequest = new RestRequest();
        reservationRequest.Resource = "reservation";
        reservationRequest.AddHeader("Cache-Control", "no-cache");
        reservationRequest.AddHeader("Authorization", $"Bearer {_accessToken}");
        reservationRequest.AlwaysMultipartFormData = false;
        reservationRequest.AddJsonBody(reservationRequestBody);

        var response = await _client.ExecuteAsync(reservationRequest, Method.Post);

        if (response.IsSuccessful)
        {
            var reservation = JsonSerializer.Deserialize<ReservationResponse>(response.Content);
            return reservation;
        }
        else
        {
            var reservation = new ReservationResponse()
            {
                Data = null,
                Errors = response.ErrorMessage,
                Metadata = response.Content
            };

            return reservation;
        }
    }

    public async Task<ReservationResponse> GetReservationStatus(string reservationId)
    {
        var reservationRequest = new RestRequest();
        reservationRequest.Resource = "reservation";
        reservationRequest.AddHeader("Cache-Control", "no-cache");
        reservationRequest.AddHeader("Authorization", $"Bearer {_accessToken}");
        reservationRequest.AlwaysMultipartFormData = false;
        reservationRequest.AddParameter("page[limit]", 1);
        reservationRequest.AddParameter("filter[id]", reservationId);

        var response = await _client.ExecuteAsync(reservationRequest, Method.Get);

        if (response.IsSuccessful)
        {
            var reservation = JsonSerializer.Deserialize<ReservationResponse>(response.Content);
            return reservation;
        }
        else
        {
            return null;
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

        var reservationRequest = new RestRequest();
        reservationRequest.Resource = "reservation";
        reservationRequest.AddHeader("Cache-Control", "no-cache");
        reservationRequest.AddHeader("Authorization", $"Bearer {_accessToken}");
        reservationRequest.AlwaysMultipartFormData = false;
        reservationRequest.AddJsonBody(reservationRequestBody);

        var response = await _client.ExecuteAsync(reservationRequest, Method.Patch);
        ReservationResponse reservationResponse;
        if (response.IsSuccessful)
        {
            reservationResponse = JsonSerializer.Deserialize<ReservationResponse>(response.Content);
            return reservationResponse;
        }
        else
        {
            reservationResponse = new ReservationResponse()
            {
                Data = null,
                Errors = response.ErrorMessage,
                Metadata = response.Content
            };
        }

        return reservationResponse;
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

        var reservationRequest = new RestRequest();
        reservationRequest.Resource = "reservation";
        reservationRequest.AddHeader("Cache-Control", "no-cache");
        reservationRequest.AddHeader("Authorization", $"Bearer {_accessToken}");
        reservationRequest.AlwaysMultipartFormData = false;
        reservationRequest.AddJsonBody(reservationRequestBody);

        var response = await _client.ExecuteAsync(reservationRequest, Method.Patch);
        if (response.IsSuccessful)
        {
            var reservationResponse = JsonSerializer.Deserialize<ReservationResponse>(response.Content);
            return reservationResponse;
        }
        else
        {
            var reservationResponse = new ReservationResponse()
            {
                Data = null,
                Errors = response.ErrorMessage,
                Metadata = response.Content
            };

            return reservationResponse;
        }
    }
}
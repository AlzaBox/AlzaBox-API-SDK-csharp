using System.Net;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using AlzaBox.API.Extensions;
using AlzaBox.API.V2.Models;
using Microsoft.AspNetCore.Http;

namespace AlzaBox.API.V2.Clients;

public class ReservationClient
{
    private readonly HttpClient _httpClient;

    public ReservationClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<ReservationsResponse> Get(string reservationId)
    {
        return await GetBase(reservationId);
    }

    public async Task<AlzaBox.API.V2.Models.ReservationsResponse> GetAll(int pageLimit = 10, int pageOffset = 0,
        string status = "")
    {
        var response = await GetBase("", pageLimit, pageOffset, status);
        return response;
    }

    private async Task<AlzaBox.API.V2.Models.ReservationsResponse> GetBase(string reservationId = "",
        int pageLimit = 10,
        int pageOffset = 0,
        string status = "")
    {
        var queryString = new QueryString();
        queryString = queryString.Add("page[limit]", pageLimit.ToString());
        queryString = queryString.Add("page[offset]", pageOffset.ToString());


        if (!string.IsNullOrWhiteSpace(reservationId))
        {
            queryString = queryString.Add("filter[Id]", reservationId);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            queryString = queryString.Add("filter[Status]", status);
        }

        return await _httpClient.GetWithQueryStringAsync<ReservationsResponse>("v2/reservations", queryString);
    }

    public async Task<ReservationsResponse> GetStatus(string reservationId)
    {
        return await GetBase(reservationId);
    }

    public async Task<ReservationResponse> Reserve(string id, int boxId, string packageNumber, int hoursFromNow,
        string reservationType = ReservationType.NonBinding, string customerPin = "")
    {
        var expirationDate = DateTime.Now.AddHours(hoursFromNow);
        var reservationResponse = await Reserve(id: id, boxId: boxId, packageNumber: packageNumber,
            reservationType: reservationType, startReservationDate: null, expirationDate: expirationDate,
            customerPin: customerPin);
        return reservationResponse;
    }

    public async Task<ReservationResponse> Reserve(string id, int boxId, string packageNumber, string reservationType,
        DateTime? startReservationDate, DateTime expirationDate, string customerPin,
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
                        Type = reservationType
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

        if (startReservationDate != null)
        {
            reservationRequestBody.Data.Reservation.Attributes.StartReservationDate =
                startReservationDate.Value.ToString("O");
        }

        if (!string.IsNullOrWhiteSpace(customerPin))
        {
            reservationRequestBody.Data.Reservation.Attributes.Pin = customerPin;
        }

        var jsonInString = JsonSerializer.Serialize(reservationRequestBody);
        var httpContent = new StringContent(jsonInString, Encoding.UTF8, "application/json");

        var responseMessage = await _httpClient.PostAsync("v2/reservation", httpContent);

        if (responseMessage.IsSuccessStatusCode)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            try
            {
                var response = JsonSerializer.Deserialize<ReservationResponse>(content);
                return response;
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Can't deserialize response: {content}", ex);
            }
        }
        else
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, responseMessage.StatusCode);
        }
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


    public async Task<ReservationResponse> Lock(string reservationId)
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
                        //Status = "STOCKED_LOCKED",
                        Blocked = true
                    },
                }
            }
        };

        return await PatchReservation(reservationRequestBody);
    }

    public async Task<ReservationResponse> Unlock(string reservationId)
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
                        //Status = "STOCKED"
                        Blocked = false
                    },
                }
            }
        };

        return await PatchReservation(reservationRequestBody);
    }

    private async Task<ReservationResponse> PatchReservation(ReservationRequest reservationRequest)
    {
        return await _httpClient.SendJsonAsync<ReservationResponse, ReservationRequest>(HttpMethod.Patch,
            "v2/reservation", reservationRequest);
    }
}
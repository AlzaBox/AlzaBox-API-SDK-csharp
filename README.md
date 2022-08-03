# AlzaBox-API-SDK-csharp
- Preview of opensource libraries for easier use of AlzaBox API.
- **Warning!** Use it for your own risk, now.
- This project is licensed under the terms of the MIT license.

### AlzaBoxClient 
- has contructor with parameters to set test or production environment.
- has login method to authenticate by your credentials
- has boxes and reservation properties which divide API methods by domain

### Methods
- Boxes.GetAll
- Boxes.Get
- Reservation.GetAll
- Reservation.Get
- Reservetion.Reserve
- Reservation.Extend
- Reservation.Cancel


# Credentials
If you don't have any credentials for connecting to AlzaBox API, please contact our business key account at ...

# Sample usage
Base commuication client class is AlzaBoxClient. If you don't use any constructor parameters, the test url for IDM will be set as well as the test url for AlzaBox API.

```csharp
var id = $"RES{Guid.NewGuid()}";
var alzaBoxClient = new AlzaBox.API.Clients.AlzaBoxClient();
var response0 = await alzaBoxClient.Login("username", "password", "clientId", "clientSecret");
var response1 = await alzaBoxClient.Boxes.GetAll();
var response2 = await alzaBoxClient.Reservations.Reserve(id, response1.Data[0].Id, $"PKG{Guid.NewGuid()}", 24);
var response3 = await alzaBoxClient.Reservations.GetReservationStatus(id);
var response4 = await alzaBoxClient.Reservations.ExtendReservation(id, 48);
var response5 = await alzaBoxClient.Reservations.CancelReservation(id);
```

# Next steps
- Completing the models property description
- Getting rid of RestSharp library dependency
- Nuget packages


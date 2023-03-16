# AlzaBox-API-SDK-csharp

Nuget package for API V1:

![Nuget](https://img.shields.io/nuget/v/Alzabox.API.SDK) 

Nuget package for API V2:

![Nuget](https://img.shields.io/nuget/v/Alzabox.API.SDK.V2)

- Preview of opensource libraries for easier use of AlzaBox API.
- **Warning!** Use it for your own risk, now.
- Its better if you fork this repository, and link forked source code, instead of link nuget packages
- This project is licensed under the terms of the MIT license.


### How to communicate?

We can communicate with each other on discord completely openly, without prejudices, without worries, but also without guarantees!

[Link to discord server](https://discord.gg/BFuDPzRxAX)

### How to be happy 
The happy path scenario is marked with a white background. In this case, you just need to use the **Reserve** method and learn to receive callbacks.

![This is an AlzaBox API SDK process diagram](https://raw.githubusercontent.com/AlzaBox/AlzaBox-API-SDK-csharp/main/AlzaBox_API_reservation_process.jpg)

### API Versions

AlzaBox API has now two versions: V1 and V2. You can see two projects and two cli examples in this reporitory.
V2 has some new features and different classes for serialization/deserialization requests and responses.

### AlzaBoxClient 
- has contructor with parameters to set test or production environment.
- has login method to authenticate by your credentials
- has boxes and reservation properties which divide API methods by domain

### Methods
API V1 and V2:
- Boxes.GetAll
- Boxes.Get
- Reservation.GetAll
- Reservation.Get
- Reservetion.Reserve
- Reservation.Extend
- Reservation.Cancel
- Reservation.Lock
- Reservation.Unlock

Only in V2:
- Courier.Get, Create, Update
- Reservation can define your own PIN for customer
- Reservation type (NON-BINDING, IMMEDIATE, TIME)
- Reservation has time reservation with startReservationDate attribute

# Credentials
If you don't have any credentials for connecting to AlzaBox API, please contact our business key account at ...

# Sample usage
Base commuication client class is AlzaBoxClient. If you don't use any constructor parameters, the test url for IDM will be set as well as the test url for AlzaBox API.

```csharp
var id = $"RES{Guid.NewGuid()}";
var alzaBoxClient = new AlzaBox.API.Clients.AlzaBoxClient();
var response0 = await alzaBoxClient.Login(args[0],args[1], args[2], args[3]);
var response1 = await alzaBoxClient.Boxes.GetAll();
var response2 = await alzaBoxClient.Reservations.Reserve(id, response1.Data[0].Id, $"PKG{Guid.NewGuid()}", 24);
var response3 = await alzaBoxClient.Reservations.GetStatus(id);
var response4 = await alzaBoxClient.Reservations.Extend(id, 48);
var response5 = await alzaBoxClient.Reservations.Lock(id);
var response6 = await alzaBoxClient.Reservations.Unlock(id);
var response7 = await alzaBoxClient.Reservations.Cancel(id);
```

Courier management examples 
```csharp
var boxes = new List<CourierBox>();
boxes.Add(new CourierBox() { Id = 1 });
await alzaBoxClient.Couriers.Create("login", "12345", CourierAccessType.Full);
await alzaBoxClient.Couriers.Get("login");
await alzaBoxClient.Couriers.Update("login", "54321", CourierAccessType.Specific, boxes);
```

# Next steps
- Completing the models property description
- DTO for Callbacks


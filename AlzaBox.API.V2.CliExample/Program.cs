using AlzaBox.API.V2.Models;
using AlzaBox.API.V2.Clients;

var reservationId1 = $"RES{Guid.NewGuid()}";
var reservationId2 = $"RES{Guid.NewGuid()}";
var reservationId3 = $"VRES{Guid.NewGuid()}";
var courierId = DateTime.Now.Millisecond.ToString();
var courierBoxes = new List<CourierBox>();

var alzaBoxClient = new AlzaBoxClient();
var loginResponse = await alzaBoxClient.Login(args[0], args[1], args[2], args[3]);

// Boxes example
var boxesReponse = await alzaBoxClient.Boxes.GetAll();
var firstBoxId = boxesReponse.Data.Boxes.FirstOrDefault().Id;
var firstBoxOccupancy = await alzaBoxClient.Boxes.GetBoxOccupancy(firstBoxId);
var boxResponse = await alzaBoxClient.Boxes.Get(firstBoxId);

// Reservations example
var getReservationsResponse = await alzaBoxClient.Reservations.GetAll();
var getReservationResponse = await alzaBoxClient.Reservations.Get(getReservationsResponse.Data.Reservations.FirstOrDefault().Id);
var createReservationResponse1 = await alzaBoxClient.Reservations.Reserve(reservationId1, 3528, $"PKG{Guid.NewGuid()}", 24);
var createReservationResponse2 = await alzaBoxClient.Reservations.Reserve(reservationId2, 3528, $"PKG{Guid.NewGuid()}", 24, ReservationType.Immediate, customerPin: "12345");
var statusResponse = await alzaBoxClient.Reservations.GetStatus(reservationId1);
var extendResponse = await alzaBoxClient.Reservations.Extend(reservationId1, 48);
var lockResponse = await alzaBoxClient.Reservations.Lock(reservationId1);
var unlockResponse = await alzaBoxClient.Reservations.Unlock(reservationId1);
var cancelResponse = await alzaBoxClient.Reservations.Cancel(reservationId1);

// Couriers example
var createCourierResponse = await alzaBoxClient.Couriers.Create("Test1", "12345", null);
var getCourierResponse = await alzaBoxClient.Couriers.Get("Test1");
var updateCourierResponse = await alzaBoxClient.Couriers.Update("Test1", "54321", null);
var getCourierResponse2 = await alzaBoxClient.Couriers.Get("Test1");

// VirtualBox example
var createVirtualReservationResponse1 = await alzaBoxClient.Reservations.Reserve(reservationId3, 100, $"PKG{Guid.NewGuid()}", 24);
var virtualBoxResponseStocked = await alzaBoxClient.VirtualBox.Stocked(reservationId3);
var virtualBoxResponsePickedUp = await alzaBoxClient.VirtualBox.PickedUp(reservationId3);

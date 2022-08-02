var reservationId = "RES" + Guid.NewGuid();
var alzaBoxClient = new AlzaBox.API.Clients.AlzaBoxClient();
var response0 =await alzaBoxClient.Login("username", "password", "clientId", "clientSecret");
var response1 = await alzaBoxClient.Boxes.GetAll();
var response2 = await alzaBoxClient.Reservations.Reserve(reservationId, response1.Data[0].Id, $"PKG{Guid.NewGuid()}", 24);
var response3 = await alzaBoxClient.Reservations.GetReservationStatus(reservationId);
var response4 = await alzaBoxClient.Reservations.ExtendReservation(reservationId, 48);
var response5 = await alzaBoxClient.Reservations.CancelReservation(reservationId);
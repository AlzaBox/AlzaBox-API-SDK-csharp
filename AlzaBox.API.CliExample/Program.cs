﻿var id = $"RES{Guid.NewGuid()}";
var alzaBoxClient = new AlzaBox.API.Clients.AlzaBoxClient();
var response0 = await alzaBoxClient.Login(args[0], args[1], args[2], args[3]);
var response1 = await alzaBoxClient.Boxes.GetAll();
var response2 = await alzaBoxClient.Reservations.Reserve(id, response1.Data[0].Id, $"PKG{Guid.NewGuid()}", 24);
var response3 = await alzaBoxClient.Reservations.GetStatus(id);
var response4 = await alzaBoxClient.Reservations.Extend(id, 48);
var response5 = await alzaBoxClient.Reservations.Cancel(id);
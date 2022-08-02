using CommandLine;
using AlzaBox.API.Models;
using AlzaBox.API.Clients;
using AlzaBox.API.CliExample;
await Parser.Default.ParseArguments<Options>(args).WithParsedAsync<Options>(async options => {
        var alzaBoxClient = new AlzaBoxClient(Constants.TestIdentityBaseUrl, Constants.TestParcelLockersBaseUrl);
        
        var reservationId = "RES"+Guid.NewGuid();
        var packageNumber = "PKG"+Guid.NewGuid();
        
        await alzaBoxClient.Login(options.Username, options.Password, options.ClientId, options.ClientSecret);
        var response1 = await alzaBoxClient.Boxes.GetAll();
        var response2 = await alzaBoxClient.Reservations.Reserve(reservationId, response1.Data[0].Id, packageNumber, 24);
        var response3 = await alzaBoxClient.Reservations.GetReservationStatus(reservationId);
        var response4 = await alzaBoxClient.Reservations.ExtendReservation(reservationId, 48);
        var response5 = await alzaBoxClient.Reservations.CancelReservation(reservationId);
    });
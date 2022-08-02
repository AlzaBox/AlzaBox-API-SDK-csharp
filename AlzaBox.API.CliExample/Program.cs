using CommandLine;
using AlzaBox.API.Models;
using AlzaBox.API.Clients;
using AlzaBox.API.CliExample;
await Parser.Default.ParseArguments<Options>(args).WithParsedAsync<Options>(async options => {
        var alzaBoxClient = new AlzaBoxClient(Constants.TestIdentityBaseUrl, Constants.TestParcelLockersBaseUrl);
        
        var expirationDate = DateTime.Now.AddHours(1);
        var extendExpirationDate = expirationDate.AddHours(2);
        var reservationId = Guid.NewGuid().ToString();
        var packageNumber = Guid.NewGuid().ToString();
        
        await alzaBoxClient.Login(options.Username, options.Password, options.ClientId, options.ClientSecret);
        var boxResponse = await alzaBoxClient.Boxes.GetAll();
        boxResponse = await alzaBoxClient.Boxes.Get(1);
        await alzaBoxClient.Reservations.Reserve(reservationId, boxResponse.Data[0].Id, packageNumber,
            expirationDate, 5, 5, 5);
        await alzaBoxClient.Reservations.GetReservationStatus(reservationId);
        await alzaBoxClient.Reservations.ExtendReservation(reservationId, extendExpirationDate);
        await alzaBoxClient.Reservations.CancelReservation(reservationId);
    });
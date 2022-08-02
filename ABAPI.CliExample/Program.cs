using RestSharp;
using CommandLine;
using ABAPI.Services;
using ABAPI.Models;
using APIAPICliExample;

var parsedOptions = Parser.Default.ParseArguments<Options>(args);
await Parser.Default.ParseArguments<Options>(args)
    .WithParsedAsync<Options>(async options =>
    {
        var credentials = new Credentials()
        {
            UserName = options.Username,
            Password = options.Password,
            ClientId = options.ClientId,
            ClientSecret = options.ClientSecret
        };
        var expirationDate = DateTime.Now.AddHours(1);
        var extendExpirationDate = expirationDate.AddHours(2);
        var reservationId = Guid.NewGuid().ToString();
        var packageNumber = Guid.NewGuid().ToString();

        var client = new RestClient($"{Constants.TestParcelLockersBaseUrl}");
        var authenticator = new ABAPI.Services.AuthenticationService(ABAPI.Models.Constants.TestIdentityBaseUrl);
        var authenticateResponse = await authenticator.Authenticate(credentials);
        var boxClient = new BoxService(client, authenticateResponse.AccessToken);
        var boxResponse = await boxClient.GetAll();
        var rSrv = new ReservationService(client, authenticateResponse.AccessToken);

        await rSrv.Reserve(reservationId, boxResponse.Data[0].Id, packageNumber,
            expirationDate, 5, 5, 5);
        await rSrv.GetReservationStatus(reservationId);
        await rSrv.ExtendReservation(reservationId, extendExpirationDate);
        await rSrv.CancelReservation(reservationId);
    });
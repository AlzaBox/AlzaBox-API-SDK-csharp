using AlzaBox.API.WebExample.Models;

namespace AlzaBox.API.WebExample.Core.Extensions;

public static class ReservationMapper
{
    public static ReservationModel Map(this ReservationModel reservationModel, AlzaBox.API.Models.Reservation reservation)
    {
        reservationModel = new ReservationModel(); 
        reservationModel.Id = reservation.Id;
        reservationModel.PackageNumber = reservation.Attributes.Packages[0].BarCode;
        reservationModel.ExpirationDate = DateTime.Parse(reservation.Attributes.ExpirationDate);
        reservationModel.BoxId = reservation.Relationships.Box.Id;
        reservationModel.Depth = reservation.Attributes.Packages[0].Depth;
        reservationModel.Width = reservation.Attributes.Packages[0].Width;
        reservationModel.Height = reservation.Attributes.Packages[0].Height;
        return reservationModel;
    } 
}
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<int>
    {
        public string UserId { get; set; } = string.Empty;
        public int ShowId { get; set; }
        public string SeatRow { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public float Price { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    }
}

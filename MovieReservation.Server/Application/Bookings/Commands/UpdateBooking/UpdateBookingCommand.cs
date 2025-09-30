using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Features.Bookings.Commands.UpdateBooking
{
    public class UpdateBookingCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public string? SeatRow { get; set; } = string.Empty;
        public int? SeatNumber { get; set; }
        public float? Price { get; set; }
        public BookingStatus? Status { get; set; }
    }
}

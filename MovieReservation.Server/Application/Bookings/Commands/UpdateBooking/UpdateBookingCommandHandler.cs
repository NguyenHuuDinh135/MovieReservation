using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Bookings.Commands.UpdateBooking
{
    public record UpdateBookingCommand : IRequest
    {
        public int Id { get; set; }
        public string? SeatRow { get; set; } = string.Empty;
        public int? SeatNumber { get; set; }
        public BookingStatus? Status { get; set; }
    }
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public UpdateBookingCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings.FindAsync(new object[] { request.Id }, cancellationToken);

            if (booking == null)
            {
                throw new Exception($"Booking not found: {request.Id}");
            }

            // Sẽ xử lý Validate trong Validator sau
            booking.SeatRow = request.SeatRow ?? booking.SeatRow;
            booking.SeatNumber = request.SeatNumber ?? booking.SeatNumber;
            booking.Status = request.Status ?? booking.Status;

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
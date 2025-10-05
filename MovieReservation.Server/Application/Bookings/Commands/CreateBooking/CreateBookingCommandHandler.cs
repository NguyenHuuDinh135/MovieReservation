using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Bookings.Commands.CreateBooking
{
    public record CreateBookingCommand : IRequest<int>
    {
        public string UserId { get; set; } = string.Empty;
        public int ShowId { get; set; }
        public string SeatRow { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public float Price { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public DateTime BookingDateTime { get; set; } = DateTime.Now;
    }

    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
    {

        private readonly IMovieReservationDbContext _context;
        public CreateBookingCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = new Booking
            {
                UserId = request.UserId,
                ShowId = request.ShowId,
                SeatRow = request.SeatRow,
                SeatNumber = request.SeatNumber,
                Price = request.Price,
                Status = request.Status
            };
            
            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync(cancellationToken);
            
            return booking.Id;
        }
    }
}
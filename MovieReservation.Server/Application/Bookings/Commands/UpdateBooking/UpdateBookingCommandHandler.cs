using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Identity.Client;
using MovieReservation.Server.Infrastructure;

namespace MovieReservation.Server.Application.Features.Bookings.Commands.UpdateBooking
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public UpdateBookingCommandHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingEntityAsync(request.Id, cancellationToken);

            if (booking == null)
            {
                throw new Exception($"Booking not found: {request.Id}");
            }

            Console.WriteLine($"{request.Id}, {request.UserId}, {request.SeatRow}, {request.SeatNumber}, {request.Price}, {request.Status}");
            // Cập nhật trực tiếp
            booking.UserId = request.UserId ?? booking.UserId;
            booking.SeatRow = request.SeatRow ?? booking.SeatRow;
            booking.SeatNumber = request.SeatNumber ?? booking.SeatNumber;
            booking.Price = request.Price ?? booking.Price;
            booking.Status = request.Status ?? booking.Status;
            await _bookingRepository.UpdateBookingAsync(booking, cancellationToken);

            return Unit.Value;
        }
    }
}
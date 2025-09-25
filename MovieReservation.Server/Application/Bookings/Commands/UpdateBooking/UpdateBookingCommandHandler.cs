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
            var booking = await _bookingRepository.GetBookingByIdAsync(request.Id, cancellationToken);

            if (booking == null)
            {
                throw new Exception($"Booking not found: {request.Id}");
            }

            if (booking.Status == request.Status || booking.SeatRow == request.SeatRow || booking.SeatNumber == request.SeatNumber || booking.Price == request.Price)
            {
                await _bookingRepository.UpdateBookingAsync(_mapper.Map<Booking>(request), cancellationToken);
            }

            return Unit.Value;
        }
    }
}
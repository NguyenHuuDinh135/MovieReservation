using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Features.Bookings.Commands.CreateBooking;

namespace MovieReservation.Server.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
    {

        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var result = await _bookingRepository.CreateBookingAsync(_mapper.Map<Booking>(request), cancellationToken);
            if (result == null)
            {
                throw new ConflictException($"Failed to create {nameof(Booking)} booking.");
            }
            return result;
        }
    }
}
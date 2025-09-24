using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieReservation.Server.Infrastructure.Data.Repositories;


namespace MovieReservation.Server.Application.Bookings.Queries.GetBookings
{
    public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, List<GetBookingsQueryResponse>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<List<GetBookingsQueryResponse>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync(cancellationToken);
            // Map to response DTOs if necessary
            List<GetBookingsQueryResponse> responses = _mapper.Map<List<GetBookingsQueryResponse>>(bookings);
            return responses;
        }
    }
}
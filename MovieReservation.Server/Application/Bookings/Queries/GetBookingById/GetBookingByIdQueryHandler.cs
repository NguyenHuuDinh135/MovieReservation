using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingById
{
    public record GetBookingByIdQuery : IRequest<BookingByIdDto>
    {
        public int Id { get; init; }
    }

    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingByIdDto>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookingByIdDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Id == request.Id)
                .ProjectTo<BookingByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (booking == null)
                throw new NotFoundException($"Booking with ID {request.Id} not found.");

            return booking;
        }
    }
}

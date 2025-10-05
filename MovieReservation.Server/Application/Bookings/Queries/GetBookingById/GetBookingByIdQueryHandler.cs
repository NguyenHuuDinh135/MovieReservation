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
    public record GetBookingByIdQuery : IRequest<GetBookingByIdDto>
    {
        public int Id { get; set; }
    }

    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, GetBookingByIdDto>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetBookingByIdDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Id == request.Id)
                .ProjectTo<GetBookingByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (booking == null)
                throw new NotFoundException($"Booking with ID {request.Id} not found.");

            return booking;
        }
    }
}

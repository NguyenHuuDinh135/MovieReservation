using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;


namespace MovieReservation.Server.Application.Bookings.Queries.GetBookings
{
    public record GetBookingsQuery : IRequest<List<GetBookingsDto>>
    {
    }
    public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, List<GetBookingsDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetBookingsQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetBookingsDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Bookings
                .AsNoTracking()
                .ProjectTo<GetBookingsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null || result.Count == 0)
                throw new NotFoundException("No bookings found.");

            return result;
        }

    }
}
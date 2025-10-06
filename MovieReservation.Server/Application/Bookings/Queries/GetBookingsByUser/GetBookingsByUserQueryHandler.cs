using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByUser
{
    public record GetBooingsByUserQuery : IRequest<List<GetBookingsByUserDto>>
    {
        public String Id { get; init; }
    }
   public class GetBookingsByUserQueryHandler : IRequestHandler<GetBooingsByUserQuery, List<GetBookingsByUserDto>>
   {
        private IMovieReservationDbContext _context;
        private IMapper _mapper;

        public GetBookingsByUserQueryHandler(IMovieReservationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetBookingsByUserDto>> Handle(GetBooingsByUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == request.Id)
                .ProjectTo<GetBookingsByUserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null)
                throw new NotFoundException($"Bookings with UserId {request.Id} not found.");

            return result;

        }
   }
}
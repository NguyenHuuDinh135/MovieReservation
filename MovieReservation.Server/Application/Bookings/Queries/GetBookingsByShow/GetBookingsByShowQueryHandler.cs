using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByShow
{
    public record GetBookingsByShowQuery : IRequest<List<BookingsByShowDto>>
    {
        public int Id { get; init; }
    }
   public class GetBookingsByShowQueryHandler : IRequestHandler<GetBookingsByShowQuery, List<BookingsByShowDto>>
   {
        private IMovieReservationDbContext _context;
        private IMapper _mapper;

        public GetBookingsByShowQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<List<BookingsByShowDto>> Handle(GetBookingsByShowQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.ShowId == request.Id)
                .ProjectTo<BookingsByShowDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null)
                throw new NotFoundException($"Booking with ShowId {request.Id} not found.");

            return result;

        }
   }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Shows.Queries.GetShows
{
    public record GetShowsQuery : IRequest<List<ShowsDto>>
    {
    }
    public class GetShowsQueryHandler : IRequestHandler<GetShowsQuery, List<ShowsDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetShowsQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ShowsDto>> Handle(GetShowsQuery shows, CancellationToken cancellationToken)
        {
            var result = await _context.Shows
                .AsNoTracking()
                .ProjectTo<ShowsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null || result.Count == 0)
                throw new NotFoundException("No bookings found.");

            return result;
        }
    }
}
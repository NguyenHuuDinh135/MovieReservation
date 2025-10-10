using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Shows.Queries.GetFilteredShows
{
    public record GetFilteredShowsQuery : IRequest<List<ShowsDto>>
    {
        public DateTime Date { get; init; }
    }
    public class GetFilteredShowsQueryHandler : IRequestHandler<GetFilteredShowsQuery, List<ShowsDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetFilteredShowsQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ShowsDto>> Handle(GetFilteredShowsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Shows
                .Where(s => s.Date == request.Date)
                .AsNoTracking()
                .ProjectTo<ShowsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (!result.Any())
                throw new NotFoundException("Shows for this date not found.");

            return result;
        }
    }
}
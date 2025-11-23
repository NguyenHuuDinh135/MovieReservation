using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Theaters.Queries.GetTheaters
{
    public record GetTheatersQuery : IRequest<List<TheatersDto>>
    {
        
    }
    public class GetTheatersQueryHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<GetTheatersQuery, List<TheatersDto>>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<TheatersDto>> Handle(GetTheatersQuery request, CancellationToken cancellationToken)
        {
            var theaters = await _context.Theaters
                .AsNoTracking()
                .ProjectTo<TheatersDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (theaters == null) throw new NotFoundException("Theaters not found");

            return theaters;
        }
    }
}
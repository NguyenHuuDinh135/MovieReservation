using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Application.Movies.Queries.GetMovies
{
    public record GetMoviesQuery : IRequest<List<MovieDto>>
    {
    }
    public class GetMoviesQueryHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<GetMoviesQuery, List<MovieDto>>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            try
{
                var result = await _context.Movies
                    .AsNoTracking()
                    .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Mapping error: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }
    }
}
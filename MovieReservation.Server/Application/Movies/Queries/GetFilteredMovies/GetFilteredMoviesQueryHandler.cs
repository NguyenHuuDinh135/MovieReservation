using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Movies.Queries.GetFilteredMovies
{
    public record FilteredMoviesQuery : IRequest<List<FilteredMoviesDto>>
    {
        public MovieType movieType { get; init; }
    }
    public class GetFilteredMoviesQueryHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<FilteredMoviesQuery, List<FilteredMoviesDto>>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<FilteredMoviesDto>> Handle(FilteredMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Where(m => m.MovieType == request.movieType)
                .ProjectTo<FilteredMoviesDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (movies == null) 
                throw new NotFoundException($"Movies with MovieType {request.movieType} not found");

            return movies;
        }
    }
}
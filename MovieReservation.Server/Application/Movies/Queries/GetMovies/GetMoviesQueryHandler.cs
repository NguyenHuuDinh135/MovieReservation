using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Movies.Queries.GetMovies
{
    public record GetMoviesQuery : IRequest<List<MoviesDto>>
    {
    }
    public class GetMoviesQueryHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<GetMoviesQuery, List<MoviesDto>>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<MoviesDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Movies
                .AsNoTracking()
                .ProjectTo<MoviesDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null)
                throw new NotFoundException("Movie not found");

            return result;
        }
    }
}
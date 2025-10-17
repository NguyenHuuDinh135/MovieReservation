using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Genres.Queries.GetGenreByMovie
{
    public record GetGenresByMovieQuery : IRequest<List<GenresByMovieDto>>
    {
        public int Id { get; init; }
    }

    public class GetGenresByMovieQueryHandler : IRequestHandler<GetGenresByMovieQuery, List<GenresByMovieDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetGenresByMovieQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GenresByMovieDto>> Handle(GetGenresByMovieQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.MovieGenres
                .AsNoTracking()
                .Where(mg => mg.MovieId == request.Id)
                .Select(mg => mg.Genre)
                .ProjectTo<GenresByMovieDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null || result.Count == 0)
                throw new NotFoundException($"No genres found for MovieId {request.Id}.");

            return result;
        }
    }
}
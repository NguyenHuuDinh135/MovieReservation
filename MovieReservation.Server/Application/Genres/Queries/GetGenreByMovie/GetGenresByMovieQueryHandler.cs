using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
namespace MovieReservation.Server.Application.Genres.Queries.GetGenreByMovie
{
    public record GetGenresByMovieQuery : IRequest<List<GenresByMovieDto>>
    {
        public int Id { get; init; }
    }
    public class GetGenresByMovieQueryHandler : IRequestHandler<GetGenresByMovieQuery, List<GenresByMovieDto>>
    {
        private IMovieReservationDbContext _context;
        private IMapper _mapper;

        public GetGenresByMovieQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GenresByMovieDto>> Handle(GetGenresByMovieQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Genres
                .AsNoTracking()
                .Where(b => b.ShowId == request.Id)
                .ProjectTo<GenresByMovieDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if(result == null)
                throw new NotFoundException($"Genre with MovieId {request.Id} not found.");
            return result;
        }
    }
}
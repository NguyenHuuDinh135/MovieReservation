using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Persons.Queries.GetPersonByMovie
{
    public record GetPersonByMovieQuery : IRequest<List<PersonByMovieDto>>
    {
        public int MovieId { get; init; }
    }

    public class GetPersonByMovieQueryHandler : IRequestHandler<GetPersonByMovieQuery, List<PersonByMovieDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetPersonByMovieQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PersonByMovieDto>> Handle(GetPersonByMovieQuery request, CancellationToken cancellationToken)
        {
            // Check if movie exists
            var movieExists = await _context.Movies
                .AnyAsync(m => m.Id == request.MovieId, cancellationToken);

            if (!movieExists)
                throw new NotFoundException($"Movie with ID {request.MovieId} not found.");

            var persons = await _context.MoviePersons
                .AsNoTracking()
                .Where(ms => ms.MovieId == request.MovieId)
                .Include(ms => ms.Person)
                .ProjectTo<PersonByMovieDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return persons;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Persons.Commands.AddPersonToMovie
{
    public record AddPersonToMovieCommand : IRequest
    {
        public int MovieId { get; init; }
        public int PersonId { get; init; }
        public RoleType RoleType { get; init; }
    }

    public class AddPersonToMovieCommandHandler : IRequestHandler<AddPersonToMovieCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public AddPersonToMovieCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(AddPersonToMovieCommand request, CancellationToken cancellationToken)
        {
            // Check if movie exists
            var movieExists = await _context.Movies
                .AnyAsync(m => m.Id == request.MovieId, cancellationToken);

            if (!movieExists)
                throw new NotFoundException($"Movie with ID {request.MovieId} not found.");

            // Check if person exists
            var personExists = await _context.Persons
                .AnyAsync(s => s.Id == request.PersonId, cancellationToken);

            if (!personExists)
                throw new NotFoundException($"Person with ID {request.PersonId} not found.");

            // Check if relationship already exists
            var existingRelation = await _context.MoviePersons
                .AnyAsync(ms => ms.MovieId == request.MovieId && ms.PersonId == request.PersonId, cancellationToken);

            if (existingRelation)
                throw new ConflictException($"Person {request.PersonId} is already associated with Movie {request.MovieId}.");

            var moviePerson = new MoviePerson
            {
                MovieId = request.MovieId,
                PersonId = request.PersonId,
                RoleType = request.RoleType
            };

            _context.MoviePersons.Add(moviePerson);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Persons.Commands.RemovePersonFromMovie
{
    public record RemovePersonFromMovieCommand : IRequest
    {
        public int MovieId { get; init; }
        public int PersonId { get; init; }
    }

    public class RemovePersonFromMovieCommandHandler : IRequestHandler<RemovePersonFromMovieCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public RemovePersonFromMovieCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(RemovePersonFromMovieCommand request, CancellationToken cancellationToken)
        {
            var moviePerson = await _context.MoviePersons
                .FirstOrDefaultAsync(ms => ms.MovieId == request.MovieId && ms.PersonId == request.PersonId, cancellationToken);

            if (moviePerson == null)
                throw new NotFoundException($"Person {request.PersonId} is not associated with Movie {request.MovieId}.");

            _context.MoviePersons.Remove(moviePerson);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

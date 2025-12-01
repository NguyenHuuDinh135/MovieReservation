using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Persons.Commands.DeletePerson
{
    public record DeletePersonCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public DeletePersonCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _context.Persons.FindAsync(new object[] { request.Id }, cancellationToken);

            if (person == null)
                throw new NotFoundException($"Person with ID {request.Id} not found.");

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

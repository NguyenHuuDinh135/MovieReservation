using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Persons.Commands.UpdatePerson
{
    public record UpdatePersonCommand : IRequest
    {
        public int Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public byte Age { get; init; }
        public string PictureUrl { get; init; } = string.Empty;
    }

    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public UpdatePersonCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _context.Persons.FindAsync(new object[] { request.Id }, cancellationToken);

            if (person == null)
                throw new NotFoundException($"Person with ID {request.Id} not found.");

            person.FullName = request.FullName;
            person.Age = request.Age;
            person.PictureUrl = request.PictureUrl;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

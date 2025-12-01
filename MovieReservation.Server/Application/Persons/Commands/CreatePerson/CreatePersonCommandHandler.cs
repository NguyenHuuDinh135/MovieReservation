using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Persons.Commands.CreatePerson
{
    public record CreatePersonCommand : IRequest<int>
    {
        public string FullName { get; init; } = string.Empty;
        public byte Age { get; init; }
        public string PictureUrl { get; init; } = string.Empty;
    }

    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, int>
    {
        private readonly IMovieReservationDbContext _context;

        public CreatePersonCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = new Person
            {
                FullName = request.FullName,
                Age = request.Age,
                PictureUrl = request.PictureUrl
            };

            _context.Persons.Add(person);
            await _context.SaveChangesAsync(cancellationToken);

            return person.Id;
        }
    }
}

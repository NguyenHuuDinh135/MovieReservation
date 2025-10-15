using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Genres.Commands.UpdateGenre
{
    public record UpdateGenreCommand : IRequest
    {
        public string name { get; set; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand>
    {
        private readonly IMovieReservationDbContext _context;
        
        public UpdateGenreCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken) {
            var Genre = await _context.Genres.FindAsync(new object[] { request.Id }, cancellationToken);

            if(genre == null)
            {
                throw new Exception($"Booking not found: {request.Id}");
            }

            // Sẽ xử lý Validate trong Validator sau
            genre.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
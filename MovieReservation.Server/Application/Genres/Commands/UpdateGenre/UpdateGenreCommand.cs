using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReservation.Server.Application.Genres.Commands.UpdateGenre;

namespace MovieReservation.Server.Application.Genres.Commands.UpdateGenre
{
    public record UpdateGenreCommand : IRequest
    {
        public int Id { get; init; }
        public string Name { get; set; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand>
    {
        private readonly IMovieReservationDbContext _context;
        
        public UpdateGenreCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken) {
            var genre = await _context.Genres.FindAsync(new object[] { request.Id }, cancellationToken);

            if(genre == null)
            {
                throw new Exception($"Genre not found: {request.Id}");
            }

            // Sẽ xử lý Validate trong Validator sau
            genre.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
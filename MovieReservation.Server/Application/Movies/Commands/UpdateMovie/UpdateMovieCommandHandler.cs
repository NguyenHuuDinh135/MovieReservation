using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Movies.Commands.UpdateMovie
{
    public record UpdateMovieCommand : IRequest<string>
    {
        public int Id { get; init; }
        public MovieType? MovieType { get; set; }
    }
   public class UpdateMovieCommandHandler(IMovieReservationDbContext context) : IRequestHandler<UpdateMovieCommand, string>
   {
        private readonly IMovieReservationDbContext _context = context;

        public async Task<string> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"Movie with Id {request.Id} not found.");

            movie.MovieType = request.MovieType ?? movie.MovieType;

            int rowChanged = await _context.SaveChangesAsync(cancellationToken);

            if (rowChanged == 0)
                throw new DbUpdateException($"Conflict when update Movie with ID {request.Id}");
            
            return $"Rows matched: {movie.Id}  Changed: {rowChanged}  Warnings: 0";
        }
   }
}
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Movies.Commands.DeleteMovie
{
    public record DeleteMovieCommand : IRequest
    {
        public int Id { get; init; }
    }
   public class DeleteMovieCommandHandler(IMovieReservationDbContext context) : IRequestHandler<DeleteMovieCommand>
   {
        private readonly IMovieReservationDbContext _context = context;

        public async Task Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie  = await _context.Movies.FindAsync([request.Id], cancellationToken) ?? throw new NotFoundException($"Movie with Id {request.Id} not found.");
            
            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync(cancellationToken);
        }
   }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Movies.Commands.CreateMovie
{
    public record CreateMovieCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string TrailerUrl { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public MovieType MovieType { get; set; }
    }
   public class CreateMovieCommandHandler(IMovieReservationDbContext context) : IRequestHandler<CreateMovieCommand, int>
   {
        private readonly IMovieReservationDbContext _context = context;

        public async Task<int> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Title = request.Title,
                Summary = request.Summary,
                Year = request.Year,
                TrailerUrl = request.TrailerUrl,
                PosterUrl = request.PosterUrl,
                MovieType = request.MovieType
            };

            _context.Movies.Add(movie);

            await _context.SaveChangesAsync(cancellationToken);
        
            return movie.Id;

        }
   }
}
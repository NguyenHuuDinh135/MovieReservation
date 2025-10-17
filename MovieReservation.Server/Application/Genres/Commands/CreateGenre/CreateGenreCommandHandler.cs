using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Genres.Commands.CreateGenre
{
    public record CreateGenreCommand : IRequest<int>
    {
        public string Name { get; init; }
    }

    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, int>
    {
        private readonly IMovieReservationDbContext _context;
        public CreateGenreCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = new Genre { Name = request.Name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync(cancellationToken);
            return genre.Id;
        }
    }
}
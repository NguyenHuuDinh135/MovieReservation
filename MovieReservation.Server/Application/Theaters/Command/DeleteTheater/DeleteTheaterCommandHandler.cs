using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Theaters.Command.DeleteTheater
{
    public record DeleteTheaterCommand : IRequest
    {
        public int TheaterId { get; init; }
    }
    public class DeleteTheaterCommandHandler(IMovieReservationDbContext context) : IRequestHandler<DeleteTheaterCommand>
    {
        private readonly IMovieReservationDbContext _context = context;

        public async Task Handle(DeleteTheaterCommand request, CancellationToken cancellationToken)
        {
            var theater = await _context.Theaters
                .FirstOrDefaultAsync(t => t.Id == request.TheaterId, cancellationToken) ?? throw new NotFoundException($"Theater with ID {request.TheaterId} not found");

            _context.Theaters.Remove(theater);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
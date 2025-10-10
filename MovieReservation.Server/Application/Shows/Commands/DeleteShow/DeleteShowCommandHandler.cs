using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Shows.Commands.DeleteShow
{
    public class DeleteShowCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteShowCommandHandler : IRequestHandler<DeleteShowCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public DeleteShowCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteShowCommand request, CancellationToken cancellationToken)
        {
            Show show = await _context.Shows.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"Show withId {request.Id} not found.");

            if (show == null)
            {
                throw new NotFoundException($"Show with Id {request.Id} not found.");

            }

            _context.Shows.Remove(show);
            
            await _context.SaveChangesAsync(cancellationToken);
        }
        
    }
}
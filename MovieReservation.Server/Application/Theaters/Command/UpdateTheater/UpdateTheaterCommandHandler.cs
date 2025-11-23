using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using Org.BouncyCastle.Crypto;

namespace MovieReservation.Server.Application.Theaters.Command.UpdateTheater
{
    public record UpdateTheaterCommand : IRequest
    {
        public int Id { get; init; }
        public TheaterType TheaterType { get; init; }
    }
    public class UpdateTheaterCommandHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<UpdateTheaterCommand>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateTheaterCommand request, CancellationToken cancellationToken)
        {
            var theater = await _context.Theaters
                .Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Theater with ID {request.Id} not found");
                
            theater.Type = request.TheaterType;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
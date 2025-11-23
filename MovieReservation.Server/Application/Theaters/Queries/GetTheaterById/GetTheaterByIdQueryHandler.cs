using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Theaters.Queries.GetTheaters;

namespace MovieReservation.Server.Application.Theaters.Queries.GetTheaterById
{
    public record GetTheaterByIdQuery : IRequest<TheatersDto>
    {
        public int Id { get; init; }
    }
    public class GetTheaterByIdQueryHandler : IRequestHandler<GetTheaterByIdQuery, TheatersDto>
    {
        private IMovieReservationDbContext _context;
        private IMapper _mapper;

        public GetTheaterByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TheatersDto> Handle(GetTheaterByIdQuery request, CancellationToken cancellationToken)
        {
            var theater = await _context.Theaters
                .AsNoTracking()
                .Where(t => t.Id == request.Id)
                .ProjectTo<TheatersDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (theater == null) throw new NotFoundException($"Theater with ID {request.Id} not found");

            return theater;
        }
    }
}
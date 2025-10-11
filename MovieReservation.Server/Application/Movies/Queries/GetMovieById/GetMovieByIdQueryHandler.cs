using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Movies.Queries.GetMovieById
{
    public record GetMovieByIdQuery : IRequest<MovieByIdDto>
    {
        public int Id { get; set; }
    }
    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieByIdDto>
    {
        private IMovieReservationDbContext _context;
        private IMapper _mapper;

        public GetMovieByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MovieByIdDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies
                .AsNoTracking()
                .ProjectTo<MovieByIdDto>(_mapper.ConfigurationProvider)
                .Where(m => m.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (movie == null)
                throw new NotFoundException($"Movie with ID {request.Id} not found");

            return movie;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Genres.Queries.GetGenres
{
    public record GetGenresQuery : IRequest<List<GenresDto>>{}

    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, List<GenresDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetGenresQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GenresDto>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Genres
                .AsNoTracking()
                .ProjectTo<GenresDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null || result.Count == 0)
                throw new NotFoundException("No genres found.");
            return result;
        }
    }
}
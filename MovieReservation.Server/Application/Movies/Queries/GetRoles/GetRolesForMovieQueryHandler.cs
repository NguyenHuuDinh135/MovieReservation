using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Movies.Queries.GetRolesForMovie
{
    public record GetRolesForMovieQuery : IRequest<RolesForMovieDto>
    {
        public int Id { get; init; }
    }
    public class GetRolesForMovieQueryHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<GetRolesForMovieQuery, RolesForMovieDto>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<RolesForMovieDto> Handle(GetRolesForMovieQuery request, CancellationToken cancellationToken)
        {
            var rolesForMovie = await _context.Movies
                .AsNoTracking()
                .Where(m => m.Id == request.Id)
                .ProjectTo<RolesForMovieDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (rolesForMovie == null) throw new NotFoundException($"Roles for Movie Id {request.Id} not found.");

            return rolesForMovie;
        }
    }
}
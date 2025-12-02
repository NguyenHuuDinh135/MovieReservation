using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Movies.Queries.GetPersonForMovie
{
    public record GetPersonForMovieQuery : IRequest<PersonsForMovieDto>
    {
        public int Id { get; init; }
    }
    public class GetPersonForMovieQueryHandler(IMovieReservationDbContext context, IMapper mapper) : IRequestHandler<GetPersonForMovieQuery, PersonsForMovieDto>
    {
        private readonly IMovieReservationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<PersonsForMovieDto> Handle(GetPersonForMovieQuery request, CancellationToken cancellationToken)
        {
            var personForMovie = await _context.Movies
                .AsNoTracking()
                .Where(m => m.Id == request.Id)
                .ProjectTo<PersonsForMovieDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (personForMovie == null) throw new NotFoundException($"Person for Movie Id {request.Id} not found.");

            return personForMovie;
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Genres.Queries.GetGenreById
{
    public record GetGenresByIdQuery : IRequest<GenresByIdDto>
    {
        public int Id { get; init; }
    }
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenresByIdQuery, GenresByIdDto>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetGenreByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GenresByIdDto> Handle(GetGenresByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres
                .AsNoTracking()
                .Where(b => b.Id == request.Id)
                .ProjectTo<GenresByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (genre == null)
                throw new NotFoundException($"Genre with ID {request.Id} not found.");
            return genre;
        }
    }
}
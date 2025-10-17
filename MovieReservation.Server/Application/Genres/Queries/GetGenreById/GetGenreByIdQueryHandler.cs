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
    public record GetGenreByIdQuery : IRequest<GenreByIdDto>
    {
        public int Id { get; init; }
    }
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreByIdDto>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetGenreByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GenreByIdDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres
                .AsNoTracking()
                .Where(b => b.Id == request.Id)
                .ProjectTo<GenreByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (genre == null)
                throw new NotFoundException($"Genre with ID {request.Id} not found.");
            return genre;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;


namespace MovieReservation.Server.Application.Shows.Queries.GetShowById
{
    public class GetShowByIdQuery : IRequest<ShowsDto>
    {
        public int Id { get; init; }
    }
    public class GetShowByIdQueryHandler : IRequestHandler<GetShowByIdQuery, ShowsDto>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;
        public GetShowByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShowsDto> Handle(GetShowByIdQuery request, CancellationToken cancellationToken)
        {
            var show = await _context.Shows
                .AsNoTracking()
                .Where(b => b.Id == request.Id)
                .ProjectTo<ShowsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (show == null)
                throw new NotFoundException($"Booking with ID {request.Id} not found.");

            return show;
        }
    }
}
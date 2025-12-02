using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Persons.Queries.GetPersons
{
    public record GetPersonsQuery : IRequest<List<PersonsDto>> { }

    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, List<PersonsDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetPersonsQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PersonsDto>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Persons
                .AsNoTracking()
                .ProjectTo<PersonsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result == null || result.Count == 0)
                throw new NotFoundException("No persons found.");
            
            return result;
        }
    }
}

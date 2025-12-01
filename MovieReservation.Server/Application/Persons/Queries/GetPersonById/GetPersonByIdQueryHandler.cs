using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Persons.Queries.GetPersonById
{
    public record GetPersonByIdQuery : IRequest<PersonByIdDto>
    {
        public int Id { get; init; }
    }

    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonByIdDto>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetPersonByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PersonByIdDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _context.Persons
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
                .ProjectTo<PersonByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (person == null)
                throw new NotFoundException($"Person with ID {request.Id} not found.");
            
            return person;
        }
    }
}

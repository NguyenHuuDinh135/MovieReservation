using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Application.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQuery : IRequest<RoleByIdDto?>
    {
        public int Id { get; set; }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleByIdDto?>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RoleByIdDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return _mapper.Map<RoleByIdDto>(role);
        }
    }
}

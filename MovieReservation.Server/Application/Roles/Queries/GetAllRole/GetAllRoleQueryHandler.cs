using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Application.Roles.Queries.GetAllRole
{
    public class GetAllRoleQuery : IRequest<List<GetAllRoleDto>> { }

    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, List<GetAllRoleDto>>
    {
        private readonly IMovieReservationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllRoleQueryHandler(IMovieReservationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetAllRoleDto>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = await _context.Roles.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<List<GetAllRoleDto>>(roles);
        }
    }
}


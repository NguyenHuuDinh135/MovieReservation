using MediatR;
using MovieReservation.Server.Domain.Entities;

namespace MovieReservation.Server.Application.Roles.Command.CreateRole
{
    public class CreateRoleCommand : IRequest<Role>
    {
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? PictureUrl { get; set; }
    }
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Role>
    {
        private readonly IMovieReservationDbContext _context;

        public CreateRoleCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = new Role
            {
                FullName = request.FullName,
                Age = (byte)request.Age,
                PictureUrl = request.PictureUrl
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync(cancellationToken);

            return role;
        }
    }
}

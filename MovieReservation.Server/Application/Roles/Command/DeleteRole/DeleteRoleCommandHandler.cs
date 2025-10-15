using MediatR;

namespace MovieReservation.Server.Application.Roles.Command.DeleteRole
{
    public class DeleteRoleCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly IMovieReservationDbContext _context;

        public DeleteRoleCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _context.Roles.FindAsync(new object[] { request.Id }, cancellationToken);
            if (role == null)
                throw new KeyNotFoundException($"Role with id {request.Id} not found");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

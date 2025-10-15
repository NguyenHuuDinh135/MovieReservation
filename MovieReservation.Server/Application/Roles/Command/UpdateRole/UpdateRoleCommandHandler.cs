using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure;

namespace MovieReservation.Server.Application.Roles.Command.UpdateRole
{
    public class UpdateRoleCommand : IRequest<object>
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? PictureUrl { get; set; }
        public UpdateRoleCommand(int id)
        {
            Id = id;
        }
        public UpdateRoleCommand() { }
    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, object>
    {
        private readonly IMovieReservationDbContext _context;

        public UpdateRoleCommandHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _context.Roles.FindAsync(new object[] { request.Id }, cancellationToken);
            if (role == null)
                throw new KeyNotFoundException($"Role with id {request.Id} not found");

            int rowsMatched = 1;
            int changed = 0;

            if (request.FullName != null && role.FullName != request.FullName)
            {
                role.FullName = request.FullName;
                changed++;
            }

            if (request.Age.HasValue && role.Age != request.Age)
            {
                role.Age = (byte)request.Age.Value;
                changed++;
            }

            if (request.PictureUrl != null && role.PictureUrl != request.PictureUrl)
            {
                role.PictureUrl = request.PictureUrl;
                changed++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new
            {
                rowsMatched,
                changed = changed > 0 ? 1 : 0,
                warnings = 0
            };
        }

    }
}

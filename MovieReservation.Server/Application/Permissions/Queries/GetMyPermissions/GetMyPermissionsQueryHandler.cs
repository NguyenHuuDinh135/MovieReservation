using System.Security.Claims;
using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Application.Permissions.Queries.GetMyPermissions
{
    public record GetMyPermissionsQuery(ClaimsPrincipal User) : IRequest<string[]>;

    public class GetMyPermissionsQueryHandler : IRequestHandler<GetMyPermissionsQuery, string[]>
    {
        public Task<string[]> Handle(GetMyPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = request.User.GetPermissions().ToArray();
            return Task.FromResult(permissions);
        }
    }
}


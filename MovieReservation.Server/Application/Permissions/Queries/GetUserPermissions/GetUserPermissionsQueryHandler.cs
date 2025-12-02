using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Application.Permissions.Queries.GetUserPermissions
{
    public record GetUserPermissionsQuery : IRequest<string[]>
    {
        public string UserId { get; init; } = string.Empty;
    }

    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, string[]>
    {
        private readonly UserManager<User> _userManager;

        public GetUserPermissionsQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string[]> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException($"User with ID {request.UserId} not found.");

            var claims = await _userManager.GetClaimsAsync(user);
            var permissions = claims
                .Where(c => c.Type == PermissionConstants.Permission)
                .Select(c => c.Value)
                .ToArray();

            return permissions;
        }
    }
}


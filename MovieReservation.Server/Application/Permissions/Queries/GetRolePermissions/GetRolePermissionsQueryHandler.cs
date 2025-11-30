using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Application.Permissions.Queries.GetRolePermissions
{
    public record GetRolePermissionsQuery : IRequest<string[]>
    {
        public string RoleId { get; init; } = string.Empty;
    }

    public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, string[]>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetRolePermissionsQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<string[]> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                throw new NotFoundException($"Role with ID {request.RoleId} not found.");

            var claims = await _roleManager.GetClaimsAsync(role);
            var permissions = claims
                .Where(c => c.Type == PermissionConstants.Permission)
                .Select(c => c.Value)
                .ToArray();

            return permissions;
        }
    }
}


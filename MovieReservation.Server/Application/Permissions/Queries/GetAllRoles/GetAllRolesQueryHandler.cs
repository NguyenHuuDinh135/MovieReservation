using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Application.Permissions.Queries.GetAllRoles
{
    public record GetAllRolesQuery : IRequest<List<AspRoleDto>>
    {
    }

    public class AspRoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public int PermissionsCount { get; set; }
    }

    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<AspRoleDto>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetAllRolesQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<AspRoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = _roleManager.Roles.ToList();
            var result = new List<AspRoleDto>();

            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var permissionsCount = claims
                    .Count(c => c.Type == PermissionConstants.Permission);

                result.Add(new AspRoleDto
                {
                    Id = role.Id,
                    Name = role.Name ?? string.Empty,
                    NormalizedName = role.NormalizedName ?? string.Empty,
                    PermissionsCount = permissionsCount
                });
            }

            return result.OrderBy(r => r.Name).ToList();
        }
    }
}

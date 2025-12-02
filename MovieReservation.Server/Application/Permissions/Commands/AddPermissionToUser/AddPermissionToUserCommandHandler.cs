using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure.Authorization;
using System.Security.Claims;

namespace MovieReservation.Server.Application.Permissions.Commands.AddPermissionToUser
{
    public record AddPermissionToUserCommand : IRequest
    {
        public string UserId { get; init; } = string.Empty;
        public string Permission { get; init; } = string.Empty;
        public ClaimsPrincipal CurrentUser { get; init; } = null!;
    }

    public class AddPermissionToUserCommandHandler : IRequestHandler<AddPermissionToUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddPermissionToUserCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Handle(AddPermissionToUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Permission))
                throw new Exception("Permission is required.");

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException($"User with ID {request.UserId} not found.");

            // Validate permission thuộc Role của người dùng
            var allPermissions = await GetAllUserPermissions(user);
            if (!allPermissions.Contains(request.Permission, StringComparer.Ordinal))
            {
                throw new NotFoundException($"This user don't have permission '{request.Permission}'.");
            }

            // Validate admin chỉ có thể cấp các quyền mà họ có (từ UserClaims trong token)
            var adminPermissions = request.CurrentUser.GetPermissions().ToList();
            if (!adminPermissions.Contains(request.Permission, StringComparer.Ordinal))
            {
                throw new ForbiddenAccessException($"You don't have permission '{request.Permission}'. You can only grant permissions that you have.");
            }

            var existingClaims = await _userManager.GetClaimsAsync(user);
            var exists = existingClaims.Any(c => 
                c.Type == PermissionConstants.Permission && 
                c.Value == request.Permission);

            if (exists)
                throw new ConflictException("Permission already assigned to user.");

            var result = await _userManager.AddClaimAsync(user, new Claim(PermissionConstants.Permission, request.Permission));
            if (!result.Succeeded)
                throw new Exception($"Failed to add permission: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        private async Task<HashSet<string>> GetAllUserPermissions(User user)
        {
            var permissions = new HashSet<string>(StringComparer.Ordinal);

            // 1. Lấy permissions từ Roles (AspNetRoleClaims)
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    var rolePermissions = roleClaims
                        .Where(c => c.Type == PermissionConstants.Permission)
                        .Select(c => c.Value);

                    foreach (var permission in rolePermissions)
                    {
                        permissions.Add(permission);
                    }
                }
            }

            // 2. Lấy permissions từ UserClaims (AspNetUserClaims)
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userPermissions = userClaims
                .Where(c => c.Type == PermissionConstants.Permission)
                .Select(c => c.Value);

            foreach (var permission in userPermissions)
            {
                permissions.Add(permission);
            }

            return permissions;
        }
    }
}


using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure.Authorization;
using System.Security.Claims;

namespace MovieReservation.Server.Application.Permissions.Commands.RemovePermissionFromRole
{
    public record RemovePermissionFromRoleCommand : IRequest
    {
        public string RoleId { get; init; } = string.Empty;
        public string Permission { get; init; } = string.Empty;
    }

    public class RemovePermissionFromRoleCommandHandler : IRequestHandler<RemovePermissionFromRoleCommand>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RemovePermissionFromRoleCommandHandler(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Handle(RemovePermissionFromRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Permission))
                throw new Exception("Permission is required.");

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                throw new NotFoundException($"Role with ID {request.RoleId} not found.");

            // Validate permission tồn tại trong PermissionConstants
            var allPermissions = PermissionConstants.Permissions.GetAll();
            if (!allPermissions.Contains(request.Permission, StringComparer.Ordinal))
            {
                throw new NotFoundException($"Permission '{request.Permission}' không tồn tại trong PermissionConstants.");
            }

            // Xóa RoleClaim
            var claim = new Claim(PermissionConstants.Permission, request.Permission);
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (!result.Succeeded)
                throw new Exception($"Failed to remove permission: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // Tự động xóa UserClaims tương ứng của tất cả users thuộc role này
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name ?? string.Empty);
            foreach (var user in usersInRole)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var matchingClaim = userClaims.FirstOrDefault(c =>
                    c.Type == PermissionConstants.Permission &&
                    c.Value == request.Permission);

                if (matchingClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, matchingClaim);
                }
            }
        }
    }
}


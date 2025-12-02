using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Infrastructure.Authorization;
using System.Security.Claims;

namespace MovieReservation.Server.Application.Permissions.Commands.AddPermissionToRole
{
    public record AddPermissionToRoleCommand : IRequest
    {
        public string RoleId { get; init; } = string.Empty;
        public string Permission { get; init; } = string.Empty;
    }

    public class AddPermissionToRoleCommandHandler : IRequestHandler<AddPermissionToRoleCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddPermissionToRoleCommandHandler(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Handle(AddPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Permission))
                throw new Exception("Permission is required.");

            // Validate permission tồn tại trong PermissionConstants
            var allPermissions = PermissionConstants.Permissions.GetAll();
            if (!allPermissions.Contains(request.Permission, StringComparer.Ordinal))
            {
                throw new NotFoundException($"Permission '{request.Permission}' không tồn tại trong PermissionConstants.");
            }

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                throw new NotFoundException($"Role with ID {request.RoleId} not found.");

            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var exists = existingClaims.Any(c => 
                c.Type == PermissionConstants.Permission && 
                c.Value == request.Permission);

            if (exists)
                throw new ConflictException("Permission already assigned to role.");

            var result = await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.Permission, request.Permission));
            if (!result.Succeeded)
                throw new Exception($"Failed to add permission: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            
            // Tự động thêm UserClaims tương ứng cho tất cả users thuộc role này
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name ?? string.Empty);
            foreach (var user in usersInRole)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var matchingClaim = userClaims.FirstOrDefault(c =>
                    c.Type == PermissionConstants.Permission &&
                    c.Value == request.Permission);

                if (matchingClaim == null)
                {
                    await _userManager.AddClaimAsync(user, new Claim(PermissionConstants.Permission, request.Permission));
                }
            }
        }
    }
}


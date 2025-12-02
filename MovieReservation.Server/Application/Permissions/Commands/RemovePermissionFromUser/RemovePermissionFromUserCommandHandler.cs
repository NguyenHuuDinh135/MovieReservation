using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure.Authorization;
using System.Security.Claims;

namespace MovieReservation.Server.Application.Permissions.Commands.RemovePermissionFromUser
{
    public record RemovePermissionFromUserCommand : IRequest
    {
        public string UserId { get; init; } = string.Empty;
        public string Permission { get; init; } = string.Empty;
        public ClaimsPrincipal CurrentUser { get; init; } = null!;
    }

    public class RemovePermissionFromUserCommandHandler : IRequestHandler<RemovePermissionFromUserCommand>
    {
        private readonly UserManager<User> _userManager;

        public RemovePermissionFromUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(RemovePermissionFromUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Permission))
                throw new Exception("Permission is required.");

            // Validate permission tồn tại trong PermissionConstants
            var allPermissions = PermissionConstants.Permissions.GetAll();
            if (!allPermissions.Contains(request.Permission, StringComparer.Ordinal))
            {
                throw new NotFoundException($"Permission '{request.Permission}' không tồn tại trong PermissionConstants.");
            }

            // Validate admin chỉ có thể xóa các quyền mà họ có (từ roles và user claims)
            var adminPermissions = request.CurrentUser.GetPermissions().ToList();
            if (!adminPermissions.Contains(request.Permission, StringComparer.Ordinal))
            {
                throw new ForbiddenAccessException($"Bạn không có quyền xóa permission '{request.Permission}' này. Chỉ có thể xóa các quyền mà bạn đã được cấp.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException($"User with ID {request.UserId} not found.");

            var claim = new Claim(PermissionConstants.Permission, request.Permission);
            var result = await _userManager.RemoveClaimAsync(user, claim);
            if (!result.Succeeded)
                throw new Exception($"Failed to remove permission: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}


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
    }

    public class AddPermissionToUserCommandHandler : IRequestHandler<AddPermissionToUserCommand>
    {
        private readonly UserManager<User> _userManager;

        public AddPermissionToUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(AddPermissionToUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Permission))
                throw new Exception("Permission is required.");

            // Validate permission tồn tại trong PermissionConstants
            var allPermissions = PermissionConstants.Permissions.GetAll();
            if (!allPermissions.Contains(request.Permission, StringComparer.Ordinal))
            {
                throw new NotFoundException($"Permission '{request.Permission}' không tồn tại trong PermissionConstants.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException($"User with ID {request.UserId} not found.");

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
    }
}


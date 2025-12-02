using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Users.Queries;
using MovieReservation.Server.Domain.Entities;

namespace MovieReservation.Server.Application.Users.Queries.GetMyUserInfo
{
    public record GetMyUserInfoQuery : IRequest<UserDto>
    {
        public string UserId { get; init; } = string.Empty;
    }

    public class GetMyUserInfoQueryHandler : IRequestHandler<GetMyUserInfoQuery, UserDto>
    {
        private readonly UserManager<User> _userManager;

        public GetMyUserInfoQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(GetMyUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
                throw new NotFoundException($"User with ID {request.UserId} not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Contact = user.Contact,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = roles.ToList()
            };
        }
    }
}


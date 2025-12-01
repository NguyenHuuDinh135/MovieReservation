using System.Security.Claims;
using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Application.Permissions.Queries.GetAssignablePermissions
{
    /// <summary>
    /// Query để lấy các permissions mà admin hiện tại có thể cấp cho user khác
    /// Chỉ trả về các permissions mà admin có (từ UserClaims trong token)
    /// </summary>
    public record GetAssignablePermissionsQuery(ClaimsPrincipal User) : IRequest<string[]>;

    public class GetAssignablePermissionsQueryHandler : IRequestHandler<GetAssignablePermissionsQuery, string[]>
    {
        public Task<string[]> Handle(GetAssignablePermissionsQuery request, CancellationToken cancellationToken)
        {
            // Lấy tất cả permissions mà admin hiện tại có (chỉ từ UserClaims trong token)
            var permissions = request.User.GetPermissions().Distinct(StringComparer.Ordinal).ToArray();
            return Task.FromResult(permissions);
        }
    }
}

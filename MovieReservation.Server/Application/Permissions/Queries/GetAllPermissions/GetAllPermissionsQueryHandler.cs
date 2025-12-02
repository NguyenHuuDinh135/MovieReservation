using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Application.Permissions.Queries.GetAllPermissions
{
    /// <summary>
    /// Query để lấy tất cả permissions từ PermissionConstants
    /// </summary>
    public record GetAllPermissionsQuery : IRequest<string[]>
    {
    }

    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, string[]>
    {
        public Task<string[]> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            // Lấy tất cả permissions từ PermissionConstants
            var allPermissions = PermissionConstants.Permissions.GetAll();
            return Task.FromResult(allPermissions);
        }
    }
}

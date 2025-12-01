using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Permissions.Commands.AddPermissionToRole;
using MovieReservation.Server.Application.Permissions.Commands.AddPermissionToUser;
using MovieReservation.Server.Application.Permissions.Commands.RemovePermissionFromRole;
using MovieReservation.Server.Application.Permissions.Commands.RemovePermissionFromUser;
using MovieReservation.Server.Application.Permissions.Queries.GetAllPermissions;
using MovieReservation.Server.Application.Permissions.Queries.GetAllRoles;
using MovieReservation.Server.Application.Permissions.Queries.GetAssignablePermissions;
using MovieReservation.Server.Application.Permissions.Queries.GetMyPermissions;
using MovieReservation.Server.Application.Permissions.Queries.GetRolePermissions;
using MovieReservation.Server.Application.Permissions.Queries.GetUserPermissions;
using MovieReservation.Server.Infrastructure.Authorization;
using MovieReservation.Server.Web.Controllers;
using System.Security.Claims;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ensure authenticated
    public class PermissionsController : BaseController
    {
        public class PermissionDto
        {
            public string Permission { get; set; } = string.Empty;
        }

        // GET api/permissions/me
        // Endpoint này không yêu cầu ManagePermissions vì user cần lấy permissions của chính mình
        // Chỉ cần authenticated (từ [Authorize] ở controller level)
        [HttpGet("me")]
        public async Task<ActionResult<string[]>> GetMyPermissions()
        {
            var result = await Sender.Send(new GetMyPermissionsQuery(User));
            return Ok(result);
        }

        // GET api/permissions/all - Lấy tất cả permissions từ PermissionConstants
        [RequirePermission("Permission.ManagePermissions")]
        [HttpGet("all")]
        public async Task<ActionResult<string[]>> GetAllPermissions()
        {
            var result = await Sender.Send(new GetAllPermissionsQuery());
            return Ok(result);
        }

        // GET api/permissions/assignable - Lấy các permissions mà admin hiện tại có thể cấp (từ roles của admin)
        [RequirePermission("Permission.ManagePermissions")]
        [HttpGet("assignable")]
        public async Task<ActionResult<string[]>> GetAssignablePermissions()
        {
            var result = await Sender.Send(new GetAssignablePermissionsQuery(User));
            return Ok(result);
        }

        // Role endpoints
        // GET api/permissions/roles/all - Lấy tất cả roles
        [RequirePermission("Permission.ManagePermissions")]
        [HttpGet("roles/all")]
        public async Task<ActionResult<List<AspRoleDto>>> GetAllRoles()
        {
            var result = await Sender.Send(new GetAllRolesQuery());
            return Ok(result);
        }

        // GET api/permissions/roles/{roleId} - Lấy permissions của một role
        // [RequirePermission("Permission.ManagePermissions")]
        [HttpGet("roles/{roleId}")]
        public async Task<ActionResult<string[]>> GetRolePermissions(string roleId)
        {
            var result = await Sender.Send(new GetRolePermissionsQuery { RoleId = roleId });
            return Ok(result);
        }

        // POST api/permissions/roles/{roleId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpPost("roles/{roleId}")]
        public async Task<IActionResult> AddPermissionToRole(string roleId, [FromBody] PermissionDto dto)
        {
            await Sender.Send(new AddPermissionToRoleCommand
            {
                RoleId = roleId,
                Permission = dto.Permission
            });
            return Ok();
        }

        // DELETE api/permissions/roles/{roleId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> RemovePermissionFromRole(string roleId, [FromBody] PermissionDto dto)
        {
            await Sender.Send(new RemovePermissionFromRoleCommand
            {
                RoleId = roleId,
                Permission = dto.Permission
            });
            return NoContent();
        }

        // User endpoints
        // GET api/permissions/users/{userId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<string[]>> GetUserPermissions(string userId)
        {
            var result = await Sender.Send(new GetUserPermissionsQuery { UserId = userId });
            return Ok(result);
        }

        // POST api/permissions/users/{userId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpPost("users/{userId}")]
        public async Task<IActionResult> AddPermissionToUser(string userId, [FromBody] PermissionDto dto)
        {
            await Sender.Send(new AddPermissionToUserCommand
            {
                UserId = userId,
                Permission = dto.Permission,
                CurrentUser = User
            });
            return Ok();
        }

        // DELETE api/permissions/users/{userId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> RemovePermissionFromUser(string userId, [FromBody] PermissionDto dto)
        {
            await Sender.Send(new RemovePermissionFromUserCommand
            {
                UserId = userId,
                Permission = dto.Permission,
                CurrentUser = User
            });
            return NoContent();
        }
    }
}

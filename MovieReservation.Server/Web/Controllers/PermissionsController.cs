using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ensure authenticated
    public class PermissionsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public PermissionsController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public class PermissionDto
        {
            public string Permission { get; set; } = string.Empty;
        }

        // GET api/permissions/me
        // Endpoint này không yêu cầu ManagePermissions vì user cần lấy permissions của chính mình
        // Chỉ cần authenticated (từ [Authorize] ở controller level)
        [HttpGet("me")]
        public IActionResult GetMyPermissions()
        {
            var permissions = User.GetPermissions().ToArray();
            return Ok(permissions);
        }

        // Role endpoints
        // GET api/permissions/roles/{roleId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpGet("roles/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            var claims = await _roleManager.GetClaimsAsync(role);
            var permissions = claims.Where(c => c.Type == PermissionConstants.Permission).Select(c => c.Value).ToArray();
            return Ok(permissions);
        }

        // POST api/permissions/roles/{roleId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpPost("roles/{roleId}")]
        public async Task<IActionResult> AddPermissionToRole(string roleId, [FromBody] PermissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Permission)) return BadRequest("Permission is required.");
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            var exists = (await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == PermissionConstants.Permission && c.Value == dto.Permission);
            if (exists) return Conflict("Permission already assigned to role.");
            var res = await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.Permission, dto.Permission));
            if (!res.Succeeded) return BadRequest(res.Errors);
            return Ok();
        }

        // DELETE api/permissions/roles/{roleId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> RemovePermissionFromRole(string roleId, [FromBody] PermissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Permission)) return BadRequest("Permission is required.");
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            var claim = new Claim(PermissionConstants.Permission, dto.Permission);
            var res = await _roleManager.RemoveClaimAsync(role, claim);
            if (!res.Succeeded) return BadRequest(res.Errors);
            return NoContent();
        }

        // User endpoints
        // GET api/permissions/users/{userId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserPermissions(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var claims = await _userManager.GetClaimsAsync(user);
            var permissions = claims.Where(c => c.Type == PermissionConstants.Permission).Select(c => c.Value).ToArray();
            return Ok(permissions);
        }

        // POST api/permissions/users/{userId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpPost("users/{userId}")]
        public async Task<IActionResult> AddPermissionToUser(string userId, [FromBody] PermissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Permission)) return BadRequest("Permission is required.");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var exists = (await _userManager.GetClaimsAsync(user)).Any(c => c.Type == PermissionConstants.Permission && c.Value == dto.Permission);
            if (exists) return Conflict("Permission already assigned to user.");
            var res = await _userManager.AddClaimAsync(user, new Claim(PermissionConstants.Permission, dto.Permission));
            if (!res.Succeeded) return BadRequest(res.Errors);
            return Ok();
        }

        // DELETE api/permissions/users/{userId}
        [RequirePermission("Permission.ManagePermissions")]
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> RemovePermissionFromUser(string userId, [FromBody] PermissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Permission)) return BadRequest("Permission is required.");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var claim = new Claim(PermissionConstants.Permission, dto.Permission);
            var res = await _userManager.RemoveClaimAsync(user, claim);
            if (!res.Succeeded) return BadRequest(res.Errors);
            return NoContent();
        }
    }
}
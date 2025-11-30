using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Users.Queries;
using MovieReservation.Server.Application.Users.Queries.GetMyUserInfo;
using MovieReservation.Server.Application.Users.Queries.GetUserById;
using MovieReservation.Server.Application.Users.Queries.GetUsers;
using MovieReservation.Server.Infrastructure.Authorization;
using MovieReservation.Server.Web.Controllers;
using System.Security.Claims;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : BaseController
    {
        // GET api/users/me - Lấy thông tin user hiện tại
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetMyUserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await Sender.Send(new GetMyUserInfoQuery { UserId = userId });
            return Ok(result);
        }

        // GET api/users/id/{id} - Lấy thông tin user theo ID (yêu cầu quyền View Users)
        [RequirePermission(PermissionConstants.Permissions.UsersView)]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var result = await Sender.Send(new GetUserByIdQuery { Id = id });
            return Ok(result);
        }

        // GET api/users/all - Lấy danh sách tất cả users (yêu cầu quyền View Users)
        [RequirePermission(PermissionConstants.Permissions.UsersView)]
        [HttpGet("all")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await Sender.Send(new GetUsersQuery());
            return Ok(result);
        }
    }
}


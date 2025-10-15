using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Roles.Queries.GetRoleById;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Application.Roles.Command.CreateRole;
using MovieReservation.Server.Application.Roles.Command.DeleteRole;
using MovieReservation.Server.Application.Roles.Command.UpdateRole;
using MovieReservation.Server.Application.Roles.Queries.GetAllRole;
using MovieReservation.Server.Application.Roles.Queries.GetAllMoviesForARole;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : BaseController
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<GetAllRoleQuery>>> GetAllRoles()
        {
            try
            {
                var result = await Sender.Send(new GetAllRoleQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }

        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<GetRoleByIdQuery>> GetRoleById(int id)
        {
            try
            {
                var result = await Sender.Send(new GetRoleByIdQuery { Id = id });
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateRole(CreateRoleCommand command)
        {
            var result = await Sender.Send(command);
            return Ok(result);
        }

        [HttpPut("update/{id:int}")]
        public async Task<ActionResult<Role>> UpdateRole(int id, [FromBody] UpdateRoleCommand command)
        {
            try
            {
                command.Id = id;
                var result = await Sender.Send(command);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            await Sender.Send(new DeleteRoleCommand { Id = id });
            return NoContent();
        }

        // ✅ Get the movie that this role belongs to
        [HttpGet("{id:int}/movie")]
        public async Task<ActionResult<GetAllMoviesForARoleQuery>> GetMovieByRole(int id)
        {
            try
            {
                var result = await Sender.Send(new GetAllMoviesForARoleQuery(id));
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

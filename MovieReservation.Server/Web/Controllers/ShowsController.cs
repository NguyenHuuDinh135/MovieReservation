using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Shows.Commands.CreateShow;
using MovieReservation.Server.Application.Shows.Commands.DeleteShow;
using MovieReservation.Server.Application.Shows.Commands.UpdateShow;
using MovieReservation.Server.Application.Shows.Queries;
using MovieReservation.Server.Application.Shows.Queries.GetFilteredShows; // hoặc GetShows, GetShowById, v.v.
using MovieReservation.Server.Application.Shows.Queries.GetShows;
using MovieReservation.Server.Web.Controllers; // Nếu BaseController ở đây

namespace MovieReservation.Server.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : BaseController // hoặc ControllerBase nếu không dùng MediatR Sender
    {
        // [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<List<ShowsDto>>> GetAllShows()
        {
            try
            {
                var result = await Sender.Send(new GetShowsQuery());
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

        // [Authorize(Roles = "Admin,User")]
        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<ShowsDto>> GetShowById(int id)
        {
            try
            {
                // Nếu bạn có GetShowByIdQuery
                // var result = await Sender.Send(new GetShowByIdQuery { Id = id });
                // return Ok(result);

                // Nếu bạn dùng GetShowsQuery để lấy 1 item
                var result = await Sender.Send(new GetShowsQuery()); // hoặc handler riêng
                var show = result.FirstOrDefault(s => s.Id == id);
                if (show == null)
                    throw new NotFoundException("Show not found.");

                return Ok(show);
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

        // [Authorize(Roles = "Admin")]
        [HttpGet("filters")] // endpoint tương ứng với curl bạn đã cung cấp
        public async Task<ActionResult<object>> GetFilteredShows([FromQuery] DateTime date)
        {
            try
            {
                var result = await Sender.Send(new GetFilteredShowsQuery { Date = date });
                return Ok(new {
                    headers = new { success = 1, message = "Success" },
                    body = result
                });
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

        // [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateShow(CreateShowCommand command)
        {
            try
            {
                var result = await Sender.Send(command);
                return CreatedAtAction(nameof(GetShowById), new { id = result }, result);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<ActionResult<Unit>> UpdateShow(UpdateShowCommand command)
        {
            try
            {
                await Sender.Send(command);
                return NoContent(); // 204
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }

        // [Authorize(Roles = "Admin")] // Bỏ comment nếu cần xác thực vai trò
        [HttpDelete("id/{id:int}")]
        public async Task<ActionResult> DeleteShow(int id)
        {
            try
            {
                await Sender.Send(new DeleteShowCommand { Id = id });
                
                // Trả về 200 OK với body chứa thông báo như trong ví dụ
                return Ok(new
                {
                    headers = new
                    {
                        success = 1,
                        message = "Show has been deleted"
                    },
                    body = new { }
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }
    }
}
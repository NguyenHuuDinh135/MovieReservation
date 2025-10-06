using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Infrastructure;
using MovieReservation.Server.Application.Common.Models;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Application.Bookings.Queries.GetBookings;
using MovieReservation.Server.Application.Bookings.Queries.GetBookingById;
using MovieReservation.Server.Application.Bookings.Commands.DeleteBooking;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Bookings.Commands.CreateBooking;
using MovieReservation.Server.Application.Bookings.Commands.UpdateBooking;
using MovieReservation.Server.Application.Bookings.Queries.GetBookingsByUser;
using MovieReservation.Server.Application.Bookings.Queries.GetBookingsByShow;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : BaseController
    {

        // [Authorize(Roles = "Admin")] 
        [HttpGet("get-all")]
        public async Task<ActionResult<List<GetBookingsQuery>>> GetAllBookings()
        {
            try
            {
                var result = await Sender.Send(new GetBookingsQuery());
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
        [HttpGet("get-by-id/{id:int}")]
        public async Task<ActionResult<GetBookingByIdDto>> GetBookingById(int id)
        {
            try
            {
                var result = await Sender.Send(new GetBookingByIdQuery { Id = id });
                return Ok(result);
            }
            catch (NotFoundException ex) // custom exception
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

        // [Authorize(Roles = "Admin")]
        [HttpGet("get-by-userid/{id}")]
        public async Task<ActionResult<GetBookingsByUserDto>> GetBookingsByUser(String id)
        {
            try
            {
                var result = await Sender.Send(new GetBooingsByUserQuery { Id = id });
                return Ok(result);
            }
            catch (NotFoundException ex) // custom exception
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

        // [Authorize(Roles = "Admin")]
        [HttpGet("get-by-showid/{id:int}")]
        public async Task<ActionResult<GetBookingsByShowDto>> GetBookingsByShow(int id)
        {
            try
            {
                var result = await Sender.Send(new GetBookingsByShowQuery { Id = id });
                return Ok(result);
            }
            catch (NotFoundException ex) // custom exception
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

        // [Authorize(Roles = "Admin,User")]
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateBooking(CreateBookingCommand command)
        {
            try
            {
                var result = await Sender.Send(command);
                return Ok(result);
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
        
        // [Authorize(Roles = "Admin,User")]
        [HttpPut("update")]
        public async Task<ActionResult<Unit>> UpdateBooking(UpdateBookingCommand command)
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

        // [Authorize(Roles = "Admin,User")]
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            try
            {
                await Sender.Send(new DeleteBookingCommand { Id = id });
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
    }
}



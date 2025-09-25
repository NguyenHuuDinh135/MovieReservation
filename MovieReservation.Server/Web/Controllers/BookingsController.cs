using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Infrastructure;
using MovieReservation.Server.Application.Common.Models;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Application.Bookings.Queries.GetBookings;
using MovieReservation.Server.Application.Movies.Queries.GetMovies;
using MovieReservation.Server.Application.Bookings.Queries.GetBookingById;
using MovieReservation.Server.Application.Features.Bookings.Commands.CreateBooking;
using MovieReservation.Server.Application.Features.Bookings.Commands.UpdateBooking;
using MovieReservation.Server.Application.Bookings.Commands.DeleteBooking;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : BaseController
    {

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<GetBookingsQueryResponse>>> GetAllBookings()
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetBookingByIdQueryResponse>> GetBookingById(int id)
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

        [HttpPost]
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

        [HttpPut]
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

        [HttpDelete("{id:int}")]
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



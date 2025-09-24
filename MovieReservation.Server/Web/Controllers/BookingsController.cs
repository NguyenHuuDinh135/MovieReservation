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

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : BaseController
    {

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<GetBookingsQueryResponse>>> getAllBookings()
        {
            return await Sender.Send(new GetBookingsQuery());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetBookingByIdQueryResponse>> getBookingById(int id)
        {
            return await Sender.Send(new GetBookingByIdQuery { Id = id });
        }

        // [HttpPost]
        // public async Task<ActionResult> createBooking(CreateBookingCommand command)
        // {
        //     return await Sender.Send(command);
        // }
    }
}



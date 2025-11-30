using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Theaters.Command.CreateTheater;
using MovieReservation.Server.Application.Theaters.Command.DeleteTheater;
using MovieReservation.Server.Application.Theaters.Command.UpdateTheater;
using MovieReservation.Server.Application.Theaters.Queries.GetTheaterById;
using MovieReservation.Server.Application.Theaters.Queries.GetTheaters;
using MovieReservation.Server.Infrastructure;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatersController : BaseController
    {

        [RequirePermission(PermissionConstants.Permissions.TheatersView)]
        [HttpGet]
        public async Task<ActionResult<List<TheatersDto>>> GetAll()
        {
            var result = await Sender.Send(new GetTheatersQuery());

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.TheatersView)]
        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<List<TheatersDto>>> GetById(int id)
        {
            var result = await Sender.Send(new GetTheaterByIdQuery { Id = id });

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.TheatersCreate)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTheaterCommand command)
        {
            var result = await Sender.Send(command);

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.TheatersEdit)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTheaterCommand command)
        {
            await Sender.Send(command);

            return NoContent();
        }

        [RequirePermission(PermissionConstants.Permissions.TheatersDelete)]
        [HttpDelete("id/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Sender.Send(new DeleteTheaterCommand { TheaterId = id });
            
            return NoContent();
        }
    }
}



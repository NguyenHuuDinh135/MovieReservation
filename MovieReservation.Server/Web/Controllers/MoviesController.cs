using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Movies.Commands.CreateMovie;
using MovieReservation.Server.Application.Movies.Commands.DeleteMovie;
using MovieReservation.Server.Application.Movies.Commands.UpdateMovie;
using MovieReservation.Server.Application.Movies.Queries;
using MovieReservation.Server.Application.Movies.Queries.GetFilteredMovies;
using MovieReservation.Server.Application.Movies.Queries.GetMovieById;
using MovieReservation.Server.Application.Movies.Queries.GetMovies;
using MovieReservation.Server.Application.Movies.Queries.GetRolesForMovie;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Infrastructure.Authorization;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : BaseController
    {

        [RequirePermission(PermissionConstants.Permissions.MoviesView)]
        [HttpGet("all")]
        public async Task<ActionResult<List<MovieDto>>> GetAll()
        {
            var result = await Sender.Send(new GetMoviesQuery{});

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.MoviesView)]
        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<MovieDto>> GetById(int id)
        {
            var result = await Sender.Send(new GetMovieByIdQuery { Id = id });

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.MoviesView)]
        [HttpGet("filtered")]
        public async Task<ActionResult<FilteredMoviesDto>> GetFilterd([FromQuery] FilteredMoviesQuery command)
        {
            var result = await Sender.Send(command);

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.MoviesView)]
        [HttpGet("id/{id:int}/roles")]
        public async Task<ActionResult<RolesForMovieDto>> GetRolesById(int id)
        {
            var result = await Sender.Send(new GetRolesForMovieQuery { Id = id });

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.MoviesCreate)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateMovieCommand command)
        {
            var result = await Sender.Send(command);

            return Ok(result);
        }

        [RequirePermission(PermissionConstants.Permissions.MoviesEdit)]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateMovieCommand command)
        {
            await Sender.Send(command);

            return NoContent();
        }

        [RequirePermission(PermissionConstants.Permissions.MoviesDelete)]
        [HttpDelete("delete/id/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Sender.Send(new DeleteMovieCommand { Id = id} );

            return NoContent();
        }
    }
}



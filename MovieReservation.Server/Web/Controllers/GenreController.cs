using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Infrastructure;
using MovieReservation.Server.Application.Common.Models;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Application.Genres.Queries.GetGenres;
using MovieReservation.Server.Application.Genres.Queries.GetGenreById;
using MovieReservation.Server.Application.Genres.Commands.DeleteGenre;
using MovieReservation.Server.Application.Genres.Commands.UpdateGenre;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Genres.Commands.CreateGenre;
using MovieReservation.Server.Application.Genres.Commands.DeleteGenre;
using MovieReservation.Server.Application.Genres.Queries.GetGenreByMovie;

namespace MovieReservation.Server.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : BaseController
    {
        // [Authorize(Roles = "Admin")] 
        [HttpGet("all")]
        public async Task<ActionResult<List<GetGenresQuery>>> GetAllGenres()
        {
            try
            {
                var result = await Sender.Send(new GetGenresQuery());
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
        public async Task<ActionResult<GenreByIdDto>> GetGenreById(int id)
        {
            try
            {
                var result = await Sender.Send(new GetGenreByIdQuery { Id = id });
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

        // [Authorize(Role = "Admin, User")]
        [HttpPost("movies/{id}")]
        public async Task<ActionResult<GenresByMovieDto>> GetGenresByMovie(int id)
        {
            var result = await Sender.Send(new GetGenresByMovieQuery { Id = id });
            return Ok(result);
        }

        // [Authorize(Role = "Admin"]
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateGenre(CreateGenreCommand command) {
            var result = await Sender.Send(command);
            return Ok(result); 
        }

        // [Authorize(Role = "Admin")]
        [HttpPut("update")]
        public async Task<ActionResult<int>> UpdateGenre(UpdateGenreCommand command) {
            await Sender.Send(command);
            return NoContent(); // 204
        }

    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Persons.Commands.AddPersonToMovie;
using MovieReservation.Server.Application.Persons.Commands.CreatePerson;
using MovieReservation.Server.Application.Persons.Commands.DeletePerson;
using MovieReservation.Server.Application.Persons.Commands.RemovePersonFromMovie;
using MovieReservation.Server.Application.Persons.Commands.UpdatePerson;
using MovieReservation.Server.Application.Persons.Queries.GetPersonById;
using MovieReservation.Server.Application.Persons.Queries.GetPersonByMovie;
using MovieReservation.Server.Application.Persons.Queries.GetPersons;
using MovieReservation.Server.Infrastructure.Authorization;
using MovieReservation.Server.Web.Controllers;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : BaseController
    {
        [RequirePermission(PermissionConstants.Permissions.PersonView)]
        [HttpGet("all")]
        public async Task<ActionResult<List<PersonsDto>>> GetAllPersons()
        {
            try
            {
                var result = await Sender.Send(new GetPersonsQuery());
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

        [RequirePermission(PermissionConstants.Permissions.PersonView)]
        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<PersonByIdDto>> GetPersonById(int id)
        {
            try
            {
                var result = await Sender.Send(new GetPersonByIdQuery { Id = id });
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

        [RequirePermission(PermissionConstants.Permissions.PersonView)]
        [HttpGet("movies/{movieId:int}")]
        public async Task<ActionResult<List<PersonByMovieDto>>> GetPersonByMovie(int movieId)
        {
            try
            {
                var result = await Sender.Send(new GetPersonByMovieQuery { MovieId = movieId });
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

        [RequirePermission(PermissionConstants.Permissions.PersonCreate)]
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreatePerson(CreatePersonCommand command)
        {
            try
            {
                var result = await Sender.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }

        [RequirePermission(PermissionConstants.Permissions.PersonEdit)]
        [HttpPut("update")]
        public async Task<ActionResult> UpdatePerson(UpdatePersonCommand command)
        {
            try
            {
                await Sender.Send(command);
                return NoContent();
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

        [RequirePermission(PermissionConstants.Permissions.PersonDelete)]
        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            try
            {
                await Sender.Send(new DeletePersonCommand { Id = id });
                return NoContent();
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

        [RequirePermission(PermissionConstants.Permissions.PersonEdit)]
        [HttpPost("movies/add")]
        public async Task<ActionResult> AddPersonToMovie(AddPersonToMovieCommand command)
        {
            try
            {
                await Sender.Send(command);
                return NoContent();
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

        [RequirePermission(PermissionConstants.Permissions.PersonEdit)]
        [HttpDelete("movies/remove")]
        public async Task<ActionResult> RemovePersonFromMovie([FromQuery] int movieId, [FromQuery] int personId)
        {
            try
            {
                await Sender.Send(new RemovePersonFromMovieCommand { MovieId = movieId, PersonId = personId });
                return NoContent();
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
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Payments.Commands.CreatePayment;
using MovieReservation.Server.Application.Payments.Commands.DeletePayment;
using MovieReservation.Server.Application.Payments.Commands.UpdatePayment;
using MovieReservation.Server.Application.Payments.Queries.GetPaymentById;
using MovieReservation.Server.Application.Payments.Queries.GetPayments;
using MovieReservation.Server.Application.Payments.Queries.GetPaymentsByUser;

namespace MovieReservation.Server.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : BaseController
    {
        // GET api/payments/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var result = await Sender.Send(new GetPaymentsQuery());

                var body = result.Select(p => new
                {
                    payment_id = p.PaymentId,
                    amount = p.Amount,
                    payment_datetime = p.PaymentDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    payment_method = p.PaymentMethod.ToLower(),
                    user_id = p.UserId,
                    show_id = p.ShowId
                });

                return Ok(new { headers = new { success = 1, message = "Success" }, body });
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

        // GET api/payments/id/{id}
        [HttpGet("id/{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var p = await Sender.Send(new GetPaymentByIdQuery { Id = id });

                var body = new
                {
                    payment_id = p.PaymentId,
                    amount = p.Amount,
                    payment_datetime = p.PaymentDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    payment_method = p.PaymentMethod.ToLower(),
                    movie = p.Movie is not null ? new { title = p.Movie.Title, poster_url = p.Movie.PosterUrl } : null
                };

                return Ok(new { headers = new { success = 1, message = "Success" }, body });
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

        // GET api/payments/users/{id}
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetPaymentsByUser(string id)
        {
            try
            {
                var result = await Sender.Send(new GetPaymentsByUserQuery { Id = id });

                var body = result.Select(p => new
                {
                    payment_id = p.PaymentId,
                    amount = p.Amount,
                    payment_datetime = p.PaymentDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    payment_method = p.PaymentMethod.ToLower(),
                    movie = p.Movie is not null ? new { title = p.Movie.Title, poster_url = p.Movie.PosterUrl } : null
                });

                return Ok(new { headers = new { success = 1, message = "Success" }, body });
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

        // POST api/payments/create
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment(CreatePaymentCommand command)
        {
            try
            {
                var id = await Sender.Send(command);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }

        // PUT api/payments/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePayment(UpdatePaymentCommand command)
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

        // DELETE api/payments/delete/{id}
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                await Sender.Send(new DeletePaymentCommand { Id = id });
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
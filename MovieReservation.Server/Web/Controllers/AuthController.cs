using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Auth.Commands.Login;
using MovieReservation.Server.Application.Auth.Commands.RefreshToken;
using MovieReservation.Server.Application.Auth.Commands.Register;
using MovieReservation.Server.Application.Auth.Commands.VerifyOtp;

namespace MovieReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            // result lúc này chỉ báo "OTP đã gửi", chưa có token
            return Ok(new { message = result });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            // result cũng chỉ báo "OTP đã gửi" hoặc "Đăng ký thành công"
            return Ok(new { message = result });
        }

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new { message = result});
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { message = result });
        }
    }
}

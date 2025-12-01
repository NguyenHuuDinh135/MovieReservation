using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Auth.Commands.Login;
using MovieReservation.Server.Application.Auth.Commands.RefreshToken;
using MovieReservation.Server.Application.Auth.Commands.Register;
using MovieReservation.Server.Application.Auth.Commands.VerifyOtp;
using MovieReservation.Server.Application.Auth.Commands.Logout;
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
            return Ok(new { message = result, email = command.Email });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            // result cũng chỉ báo "OTP đã gửi" hoặc "Đăng ký thành công"
            return Ok(new { message = result, email = command.Email });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command)
        {
            // try catch có thể được thêm vào để xử lý lỗi và trả về mã trạng thái phù hợp
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new
                {
                    message = "Xác thực OTP thành công.",
                    succeeded = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return BadRequest(new
                {
                    message = "Xác thực OTP thất bại.",
                    succeeded = false,
                    error = ex.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand? command)
        {
            // Hỗ trợ hai luồng client:
            // 1) Client gửi refresh token trong body dưới dạng { refreshToken: '...' }
            // 2) Client dựa vào cookie HttpOnly 'refreshToken' (ưu tiên cho bảo mật)
            try
            {
                var refreshToken = command?.RefreshToken;

                // Nếu không có trong body, kiểm tra cookie
                if (string.IsNullOrEmpty(refreshToken))
                {
                    refreshToken = Request.Cookies["refreshToken"];
                }

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest(new
                    {
                        message = "Refresh token not provided.",
                        succeeded = false
                    });
                }

                // Chuyển tiếp một lệnh cụ thể được tạo từ token đã xác định
                var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));
                return Ok(new
                {
                    message = "Làm mới token thành công.",
                    succeeded = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return BadRequest(new
                {
                    message = "Làm mới token thất bại.",
                    succeeded = false,
                    error = ex.Message
                });
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}

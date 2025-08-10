using Microsoft.AspNetCore.Mvc;
using MovieReservation.Server.Application.Dtos;
using MovieReservation.Server.Application.Services;

namespace MovieReservation.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Bước 1: Đăng nhập và gửi OTP
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var otpSent = await _authService.LoginAsync(loginDto);
            if (!otpSent)
                return Unauthorized(new { message = "Invalid credentials or email not confirmed." });

            return Ok(new { message = "OTP sent successfully" });
        }

        /// <summary>
        /// Bước 2: Xác thực OTP để lấy JWT token
        /// </summary>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpDto otpDto)
        {
            var token = await _authService.VerifyOtpAndGenerateTokenAsync(otpDto);
            if (token == null)
                return Unauthorized(new { message = "Invalid or expired OTP." });

            return Ok(new { token });
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _authService.RegisterAsync(registerDto);
            if (user == null)
                return BadRequest(new { message = "Email already exists or registration failed." });

            return Ok(new { message = "Registration successful" });
        }
    }
}

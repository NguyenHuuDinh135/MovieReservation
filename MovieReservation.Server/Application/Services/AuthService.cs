using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Dtos;
using System.Collections.Concurrent;

namespace MovieReservation.Server.Application.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;

        // Lưu OTP trong bộ nhớ tạm: Email -> (Code, Expiry)
        private static readonly ConcurrentDictionary<string, (string Code, DateTime Expiry)> _otpStore = new();

        public AuthService(
            UserManager<User> userManager,
            JwtService jwtService,
            EmailService emailService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        /// <summary>
        /// Bước 1: Đăng nhập, kiểm tra tài khoản + mật khẩu, gửi OTP qua email.
        /// </summary>
        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return false;

            //if (!user.EmailConfirmed)
            //    return false;

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return false;

            return await SendOtpAsync(loginDto.Email);
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        public async Task<User?> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                return null;

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                EmailConfirmed = true // hoặc false nếu muốn flow xác thực email riêng
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return null;

            return user;
        }

        /// <summary>
        /// Gửi OTP qua email
        /// </summary>
        public async Task<bool> SendOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(5);

            _otpStore[email] = (otp, expiry);

            var emailDto = new EmailDto
            {
                To = email,
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: <b>{otp}</b>. It expires in 5 minutes."
            };
            await _emailService.SendEmailAsync(emailDto);

            return true;
        }

        /// <summary>
        /// Kiểm tra OTP có hợp lệ không
        /// </summary>
        public Task<bool> ValidateOtpAsync(OtpDto otpDto)
        {
            if (!_otpStore.TryGetValue(otpDto.Email, out var entry))
                return Task.FromResult(false);

            if (entry.Expiry < DateTime.UtcNow)
            {
                _otpStore.TryRemove(otpDto.Email, out _);
                return Task.FromResult(false);
            }

            var isValid = entry.Code == otpDto.Code;
            if (isValid)
                _otpStore.TryRemove(otpDto.Email, out _); // Xóa OTP sau khi xác thực

            return Task.FromResult(isValid);
        }

        /// <summary>
        /// Bước 2: Xác thực OTP và trả về JWT
        /// </summary>
        public async Task<string?> VerifyOtpAndGenerateTokenAsync(OtpDto otpDto)
        {
            var isValid = await ValidateOtpAsync(otpDto);
            if (!isValid)
                return null;

            var user = await _userManager.FindByEmailAsync(otpDto.Email);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtService.GenerateToken(user, roles.ToArray());
        }
    }
}

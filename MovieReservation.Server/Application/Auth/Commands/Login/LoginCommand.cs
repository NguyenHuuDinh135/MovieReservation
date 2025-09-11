using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Application.Common.Models;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure.Services;
using System.Security.Claims;

namespace MovieReservation.Server.Application.Auth.Commands.Login
{
    public record LoginCommand : IRequest<Result>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IRedisService _redisService;
        public LoginCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRedisService redisService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _redisService = redisService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Tài khoản hoặc mật khẩu không đúng.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw new Exception("Tài khoản hoặc mật khẩu không đúng.");
            var otp = _emailService.GenerateOtp();
            await _redisService.SetAsync($"otp:{user.Id}", otp, TimeSpan.FromMinutes(5));

            // Gửi email OTP
            var emailDto = new EmailDto
            {
                ToEmail = user.Email!,
                Subject = "Xác thực OTP đăng nhập",
                Body = $"Mã OTP đăng nhập của bạn là: <b>{otp}</b>. OTP sẽ hết hạn sau 5 phút."
            };
            await _emailService.SendEmailAsync(emailDto);

            return Result.Success();
            
        }
    }
}
//// Lấy IP hiện tại
//var currentIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

//// Kiểm tra OTP
//if (string.IsNullOrEmpty(request.OtpCode))
//{
//    // Sinh OTP và gửi email
//    var otp = _emailService.GenerateOtp();
//    user.OtpCode = otp;
//    user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
//    await _userManager.UpdateAsync(user);

//    await _emailService.SendEmailAsync(new EmailDto
//    {
//        ToEmail = user.Email,
//        Subject = "Mã xác thực đăng nhập",
//        Body = $"Mã OTP của bạn là: <b>{otp}</b>. Mã có hiệu lực trong 5 phút."
//    }, cancellationToken);

//    throw new Exception("Vui lòng nhập mã OTP được gửi tới email.");
//}
//else
//{
//    // Kiểm tra OTP
//    if (user.OtpCode != request.OtpCode || user.OtpExpiry < DateTime.UtcNow)
//        throw new Exception("Mã OTP không hợp lệ hoặc đã hết hạn.");

//    // Xóa OTP sau khi xác thực
//    user.OtpCode = null;
//    user.OtpExpiry = null;
//    await _userManager.UpdateAsync(user);
//}

//// Lấy roles
//var roles = await _userManager.GetRolesAsync(user);

//// Sinh Access Token & Refresh Token
//var accessToken = _jwtService.GenerateAccessToken(user, roles.ToArray());
//var refreshToken = _jwtService.GenerateRefreshToken(user);

//// Lưu Refresh Token vào User và Redis
//user.RefreshToken = refreshToken;
//user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenDays);
//user.CurrentIpAddress = currentIp;
//await _userManager.UpdateAsync(user);

//// Lưu refresh token vào Redis (key: "refresh_{userId}_{refreshToken}")
//var redisKey = $"refresh_{user.Id}_{refreshToken}";
//await _redisService.SetAsync(redisKey, new
//{
//    UserId = user.Id,
//    RefreshToken = refreshToken,
//    IpAddress = currentIp,
//    Expiry = user.RefreshTokenExpiryTime
//}, TimeSpan.FromDays(_jwtService.RefreshTokenDays));

//// Blacklist các refresh token cũ nếu có nhiều RT hoặc IP thay đổi
//var oldIp = user.CurrentIpAddress;
//if (!string.IsNullOrEmpty(oldIp) && oldIp != currentIp)
//{
//    // Gửi email cảnh báo
//    await _emailService.SendEmailAsync(new EmailDto
//    {
//        ToEmail = user.Email,
//        Subject = "Cảnh báo đăng nhập bất thường",
//        Body = $"Phát hiện đăng nhập từ IP mới: {currentIp}. Nếu không phải bạn, hãy đổi mật khẩu ngay."
//    }, cancellationToken);

//    // Xóa các refresh token cũ trong Redis
//    // (Có thể implement thêm logic quét các key refresh_{userId}_* và xóa)
//}

//// Trả về Access Token, thời gian hết hạn
//return new LoginResult
//{
//    AccessToken = accessToken,
//    ExpiresIn = _jwtService.AccessTokenMinutes * 60
//};
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

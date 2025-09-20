using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Application.Common.Models;
namespace MovieReservation.Server.Application.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<Result>
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IRedisService _redisService;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IRedisService redisService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _redisService = redisService;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
                throw new Exception("Email đã tồn tại.");
            //
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors.Select(e => e.Description)));
            }
            var otp = _emailService.GenerateOtp();
            await _redisService.SetAsync($"otp:{user.Id}", otp, TimeSpan.FromMinutes(5));

            // Gửi email OTP
            var emailDto = new EmailDto
            {
                ToEmail = user.Email!,
                Subject = "Xác thực OTP đăng ký",
                Body = $"Mã OTP của bạn là: <b>{otp}</b>. OTP sẽ hết hạn sau 5 phút."
            };
            await _emailService.SendEmailAsync(emailDto);

            return Result.Success();
        }
    }
}
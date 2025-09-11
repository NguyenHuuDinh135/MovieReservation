using Azure.Core;
using MediatR;
namespace MovieReservation.Server.Application.Auth.Commands.VerifyOtp
{
    public record VerifyOtpCommand : IRequest<Result>
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }

    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IRedisService _redisService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VerifyOtpCommandHandler(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            IRedisService redisService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _redisService = redisService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result.Failure(new[] { "Tài khoản không tồn tại." });

            var storedOtp = await _redisService.GetAsync<string>($"otp:{user.Id}");
            if (storedOtp != request.OtpCode)
                return Result.Failure(new[] { "OTP không hợp lệ." });

            await _redisService.RemoveAsync($"otp:{user.Id}");

            string accessToken, refreshToken;

            // Kiểm tra xem đã có refresh token hợp lệ chưa
            var existingRefreshToken = await _redisService.GetByUserIdAsync(user.Id);

            if (!string.IsNullOrEmpty(existingRefreshToken) &&
                await _redisService.ExistsAsync($"refresh:{existingRefreshToken}"))
            {
                // Sử dụng RT hiện có
                refreshToken = existingRefreshToken;
                var roles = await _userManager.GetRolesAsync(user);
                accessToken = _jwtService.GenerateAccessToken(user, roles.ToArray());
            }
            else
            {
                // Tạo token mới
                var roles = await _userManager.GetRolesAsync(user);
                accessToken = _jwtService.GenerateAccessToken(user, roles.ToArray());
                refreshToken = _jwtService.GenerateRefreshToken(user);

                // Store refresh token in Redis
                await _redisService.SetAsync($"refresh:{refreshToken}", user.Id,
                    TimeSpan.FromDays(_jwtService.RefreshTokenDays));
            }


            return Result.Success();
        }
    }
}
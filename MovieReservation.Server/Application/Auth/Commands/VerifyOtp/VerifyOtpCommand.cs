using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MovieReservation.Server.Application.Auth.Commands.VerifyOtp
{
    // DTO class để đảm bảo serialization ổn định
    public class TokenResponseDto
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }

    public record VerifyOtpCommand : IRequest<TokenResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }

    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, TokenResponseDto>
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

        public async Task<TokenResponseDto> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Tài khoản không tồn tại.");
            //username
            var storedOtp = await _redisService.GetAsync<string>($"otp:{user.Id}");
            if (storedOtp != request.OtpCode)
                throw new Exception("OTP không hợp lệ.");

            await _redisService.RemoveAsync($"otp:{user.Id}");
            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    throw new Exception($"Không thể xác thực email: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
            }

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

            // Cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenDays),
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            
            // Trả về DTO trực tiếp
            return new TokenResponseDto
            {
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                AccessToken = accessToken,
                ExpiresIn = _jwtService.AccessTokenMinutes
            };
        }
    }
}
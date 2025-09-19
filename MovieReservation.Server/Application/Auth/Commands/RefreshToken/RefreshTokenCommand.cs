using Azure.Core;

namespace MovieReservation.Server.Application.Auth.Commands.RefreshToken
{
    // DTO class để đảm bảo serialization ổn định
    public class TokenResponseDto
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }

    public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponseDto>;
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRedisService _redisService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RefreshTokenCommandHandler(
            UserManager<User> userManager,
            IRedisService redisService,
            IJwtService jwtService,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _redisService = redisService;
            _jwtService = jwtService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = request.RefreshToken;
            var userId = await _redisService.GetAsync<string>($"refresh:{refreshToken}");
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Refresh token không hợp lệ hoặc đã hết hạn.");

            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtService.GenerateAccessToken(user, roles.ToArray());
            var newRefreshToken = _jwtService.GenerateRefreshToken(user);

            await _redisService.RemoveAsync($"refresh:{refreshToken}");
            await _redisService.SetAsync($"refresh:{newRefreshToken}", userId, TimeSpan.FromDays(_jwtService.RefreshTokenDays));

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenDays),
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

            return new TokenResponseDto
            {
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                AccessToken = newAccessToken,
                ExpiresIn = _jwtService.AccessTokenMinutes
            };
        }
    }
}

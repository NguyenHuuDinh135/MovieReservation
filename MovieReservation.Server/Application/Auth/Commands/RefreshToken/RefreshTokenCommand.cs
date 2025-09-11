namespace MovieReservation.Server.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<Result>;
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRedisService _redisService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        public RefreshTokenCommandHandler(
            UserManager<User> userManager,
            IRedisService redisService,
            IJwtService jwtService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _redisService = redisService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = request.RefreshToken;
            var userId = await _redisService.GetAsync<string>($"refresh:{refreshToken}");
            if (string.IsNullOrEmpty(userId))
                return Result.Failure(new[] { "Refresh token không hợp lệ hoặc đã hết hạn." });

            // Tạo token mới
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtService.GenerateAccessToken(user, roles.ToArray());
            var newRefreshToken = _jwtService.GenerateRefreshToken(user);
            // Xóa RT cũ
            await _redisService.RemoveAsync($"refresh:{refreshToken}");

            // Lưu RT mới
            await _redisService.SetAsync($"refresh:{newRefreshToken}", userId, TimeSpan.FromDays(_jwtService.RefreshTokenDays));
            // Trả về token mới (có thể trả kèm trong Result nếu cần)
            return Result.Success();
        }
    }
}

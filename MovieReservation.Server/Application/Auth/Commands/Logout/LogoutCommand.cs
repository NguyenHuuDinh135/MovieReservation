// LogoutCommandHandler.cs
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MovieReservation.Server.Application.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<Result>;
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IRedisService _redisService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(
            IRedisService redisService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LogoutCommandHandler> logger)
        {
            _redisService = redisService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new Exception();
            }

            // Lấy refreshToken từ cookie
            if (context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                // Xóa refresh token trong Redis
                await _redisService.RemoveAsync($"refresh:{refreshToken}");
                _logger.LogInformation("Refresh token revoked in Redis for token: {Token}", refreshToken);
            }

            // Xóa cookie refreshToken
            context.Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            });

            _logger.LogInformation("Refresh token cookie deleted.");

            return Result.Success();
        }
    }
}
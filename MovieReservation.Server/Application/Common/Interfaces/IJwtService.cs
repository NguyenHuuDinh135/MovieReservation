namespace MovieReservation.Server.Application.Common.Interfaces
{
    public interface IJwtService
    {
        int AccessTokenMinutes { get; }
        int RefreshTokenDays { get; }
        string GenerateAccessToken(User user, string[] roles);
        string GenerateRefreshToken(User user);
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Server.Application.Common.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieReservation.Server.Infrastructure.Services
{
    // Dịch vụ JWT để tạo và quản lý token
    public class JwtService : IJwtService
    {
        // Biến thành viên để lưu trữ cấu hình
        private readonly IConfiguration _configuration;
        // Hàm khởi tạo nhận cấu hình qua Dependency Injection
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int AccessTokenMinutes => int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]);
        public int RefreshTokenDays => int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]);
        // Phương thức để tạo token truy cập JWT
        public string GenerateAccessToken(User user, string[] roles)
        {
            // Tạo các claims cho token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // Thêm các claims cho vai trò người dùng
            var roleClaims = roles != null
                ? Array.ConvertAll(roles, r => new Claim(ClaimTypes.Role, r))
                : Array.Empty<Claim>();
            // Kết hợp tất cả các claims
            var allClaims = new Claim[claims.Length + roleClaims.Length];
            claims.CopyTo(allClaims, 0);
            roleClaims.CopyTo(allClaims, claims.Length);
            // Tạo khóa bảo mật và thông tin đăng ký ký
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Tạo token JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: allClaims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"])),
                signingCredentials: creds
            );
            // Trả về token dưới dạng chuỗi
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // Phương thức để tạo token làm mới
        public string GenerateRefreshToken(User user)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}


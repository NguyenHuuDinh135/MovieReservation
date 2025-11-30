using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Infrastructure.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieReservation.Server.Infrastructure.Services
{
    // Dịch vụ JWT để tạo và quản lý token
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public JwtService(
            IConfiguration configuration,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public int AccessTokenMinutes => int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]);
        public int RefreshTokenDays => int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]);

        // Synchronous API retained for compatibility with existing callers.
        // Identity async calls are used synchronously here to merge permissions into the token.
        public string GenerateAccessToken(User user, string[] roles)
        {
            var tokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (roles != null)
            {
                foreach (var r in roles)
                {
                    tokenClaims.Add(new Claim(ClaimTypes.Role, r));
                }
            }

            // Collect permission claims from user-level claims (AspNetUserClaims)
            var userClaims = _userManager.GetClaimsAsync(user).GetAwaiter().GetResult()
                ?? Enumerable.Empty<Claim>();
            var permissionSet = new HashSet<string>(userClaims
                .Where(c => string.Equals(c.Type, PermissionConstants.Permission, StringComparison.Ordinal))
                .Select(c => c.Value));

            // Collect permission claims from roles (AspNetRoleClaims via RoleManager)
            if (roles != null)
            {
                foreach (var roleName in roles.Distinct())
                {
                    var role = _roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
                    if (role == null) continue;
                    var roleClaims = _roleManager.GetClaimsAsync(role).GetAwaiter().GetResult()
                        ?? Enumerable.Empty<Claim>();
                    foreach (var rc in roleClaims.Where(c => string.Equals(c.Type, PermissionConstants.Permission, StringComparison.Ordinal)))
                    {
                        permissionSet.Add(rc.Value);
                    }
                }
            }

            // Add permission claims (deduplicated)
            foreach (var p in permissionSet)
            {
                tokenClaims.Add(new Claim(PermissionConstants.Permission, p));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: tokenClaims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"])),
                signingCredentials: creds
            );

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


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Infrastructure.Authorization;
using MovieReservation.Server.Infrastructure.Services;
using StackExchange.Redis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieReservation.Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MovieReservationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetValue<string>("Redis:ConnectionString"), true);
                configuration.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddScoped<MovieReservationDbContextInitialiser>();

            builder.Services.AddScoped<IMovieReservationDbContext>(provider => 
                provider.GetRequiredService<MovieReservationDbContext>());

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<MovieReservationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.MapInboundClaims = false; // Giữ nguyên claim types từ token
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // Không cho phép trễ hạn
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
                    // Map tất cả claims từ JWT token vào ClaimsPrincipal
                    // MapInboundClaims = false, // Giữ nguyên claim types từ token
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };
                
                // Đảm bảo tất cả claims từ token được map vào User
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Claims đã được map tự động vào context.Principal
                        // RequirePermissionAttribute sẽ kiểm tra claims từ context.Principal
                        return Task.CompletedTask;
                    }
                };
            });

            // Authorization được xử lý bởi RequirePermissionAttribute (IAsyncAuthorizationFilter)
            // Không cần đăng ký policies vì attribute tự kiểm tra permissions từ claims
            builder.Services.AddAuthorization();

            //custom services
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IRedisService, RedisService>();


            // Cookie settings cho Refresh Token
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // chỉ gửi qua HTTPS
                options.Cookie.SameSite = SameSiteMode.Strict; // chống CSRF
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = false;
            });
        }

    }
}
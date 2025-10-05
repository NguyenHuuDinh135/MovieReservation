using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Infrastructure.Services;
using StackExchange.Redis;

namespace MovieReservation.Server.Infrastructure
{
    // Lớp mở rộng để đăng ký và cấu hình các dịch vụ tầng hạ tầng vào Dependency Injection
    public static class DependencyInjection
    {
        // cấu hình tất cả service
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {
            // cấu hình Dbcontext để kết nối đến csdl
            builder.Services.AddDbContext<MovieReservationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // cấu hình redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetValue<string>("Redis:ConnectionString"), true);
                // không dừng nếu redis chưa sẵn sàng
                configuration.AbortOnConnectFail = false; 
                return ConnectionMultiplexer.Connect(configuration);
            });

            // seeder data
            builder.Services.AddScoped<MovieReservationDbContextInitialiser>();

            // cấu hình ASP.Net
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6; // độ dài tối thiểu
                options.Password.RequireDigit = false; // không bắt buộc có số
                options.Password.RequireUppercase = false; // khôg bắt buộc in hoa
                options.Password.RequireNonAlphanumeric = false; // không bắt buộc ký tự đặt biệt

                // xác thực email trước khi đăng nhập
                options.SignIn.RequireConfirmedEmail = true;
            })
            // Kết nối Identity với DbContext
            .AddEntityFrameworkStores<MovieReservationDbContext>()
             // Thêm bộ tạo Token mặc định
            .AddDefaultTokenProviders();

            // Cấu hình Authentication sử dụng JWT Bearer
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true; // Lưu token trong HttpContext khi xác thực thành công
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // Không cho phép trễ hạn
                    ValidateIssuer = true, // Xác minh Issuer
                    ValidateAudience = true, // Xác minh Audience
                    ValidateLifetime = true, // Kiểm tra hạn token
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        // Khóa bí mật ký token
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
                };
            });

            // Bật Authorization
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
                options.ExpireTimeSpan = TimeSpan.FromDays(7); //cookie hết hạn sau 7 ngày
                options.SlidingExpiration = false; không gia hạn thời gian
            });
        }

    }
}

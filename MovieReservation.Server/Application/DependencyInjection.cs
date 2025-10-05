using FluentValidation;
using Microsoft.Extensions.Hosting;
using MovieReservation.Server.Application.Auth.Commands.Login;
using MovieReservation.Server.Application.Auth.Commands.Register;
using MovieReservation.Server.Application.Auth.Commands.VerifyOtp;
using MovieReservation.Server.Application.Auth.Commands.RefreshToken;
using MovieReservation.Server.Application.Auth.Commands.Logout;
using System.Reflection;
using MovieReservation.Server.Application.Common.Behaviours;

namespace MovieReservation.Server.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(VerifyOtpCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RefreshTokenCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(LogoutCommandHandler).Assembly);
            });
            
            // Register Validators from specific assemblies
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register MediatR handlers from specific assemblies
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            
        }
    }
}

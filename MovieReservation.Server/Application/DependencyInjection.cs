using FluentValidation;
using Microsoft.Extensions.Hosting;
using MovieReservation.Server.Application.Auth.Commands.Login;
using MovieReservation.Server.Application.Auth.Commands.Register;
using MovieReservation.Server.Application.Auth.Commands.VerifyOtp;
using MovieReservation.Server.Application.Auth.Commands.RefreshToken;
using System.Reflection;

namespace MovieReservation.Server.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VerifyOtpCommandHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RefreshTokenCommandHandler).Assembly));
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }
}

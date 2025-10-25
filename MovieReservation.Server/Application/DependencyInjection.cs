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
            // Register AutoMapper profiles from specific assemblies
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
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

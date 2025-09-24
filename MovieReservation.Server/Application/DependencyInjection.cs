using FluentValidation;
using Microsoft.Extensions.Hosting;
using MovieReservation.Server.Application.Auth.Commands.Login;
using MovieReservation.Server.Application.Auth.Commands.Register;
using MovieReservation.Server.Application.Auth.Commands.VerifyOtp;
using MovieReservation.Server.Application.Auth.Commands.RefreshToken;
using MovieReservation.Server.Application.Auth.Commands.Logout;
using System.Reflection;
using MovieReservation.Server.Application.Bookings.Queries.GetBookings;
using MovieReservation.Server.Application.Movies.Queries.GetMovies;
using MovieReservation.Server.Application.Common.Mappings;
using MovieReservation.Server.Application.Bookings.Queries.GetBookingById;

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
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LogoutCommandHandler).Assembly));
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Booking Queries
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetBookingsQueryHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetBookingByIdQueryHandler).Assembly));

            // Movie Queries
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetMoviesQueryHandler).Assembly));


            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            

            builder.Services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Application.Common.Interfaces
{
    public interface IMovieReservationDbContext
    {
        DbSet<Booking> Bookings { get; set; }
        DbSet<Show> Shows { get; set; }
        DbSet<Movie> Movies { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<MovieGenre> MovieGenres { get; set; }
        DbSet<MovieRole> MovieRoles { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Theater> Theaters { get; set; }
        DbSet<TheaterSeat> TheaterSeats { get; set; }
        DbSet<Payment> Payments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
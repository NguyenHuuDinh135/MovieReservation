using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Domain.Entities;

namespace MovieReservation.Server.Infrastructure.Data
{
    public class MovieReservationDbContext : DbContext
    {
        public MovieReservationDbContext(DbContextOptions<MovieReservationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieRole> MovieRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<TheaterSeat> TheaterSeats { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite keys
            modelBuilder.Entity<MovieGenre>().HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.Entity<MovieRole>().HasKey(mr => new { mr.MovieId, mr.RoleId });
            modelBuilder.Entity<TheaterSeat>().HasKey(ts => new { ts.SeatRow, ts.SeatNumber, ts.TheaterId });
            modelBuilder.Entity<OtpCode>().HasKey(o => o.UserId);

            // Explicit precision for Movie.Rating
            modelBuilder.Entity<Movie>()
                .Property(m => m.Rating)
                .HasPrecision(3, 1);
        }
    }
}

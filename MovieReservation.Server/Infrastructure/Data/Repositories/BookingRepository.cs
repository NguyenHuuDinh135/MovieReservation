using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Domain.Entities;

namespace MovieReservation.Server.Infrastructure.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        
        private readonly MovieReservationDbContext _dbContext;

        public BookingRepository(MovieReservationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Bookings
            .Select(b => new Booking
            {
                Id = b.Id,
                UserId = b.UserId,
                ShowId = b.ShowId,
                SeatRow = b.SeatRow,
                SeatNumber = b.SeatNumber,
                Price = b.Price,
                Status = b.Status,
                BookingDateTime = b.BookingDateTime,
                User = new User
                {
                    Id = b.User.Id,
                    Email = b.User.Email,
                    UserName = b.User.UserName
                },
                Show = new Show
                {
                    Id = b.Show.Id,
                    Date = b.Show.Date,
                    StartTime = b.Show.StartTime,
                    EndTime = b.Show.EndTime,
                    Movie = new Movie
                    {
                        Id = b.Show.Movie.Id,
                        Title = b.Show.Movie.Title
                    },
                    Theater = new Theater
                    {
                        Id = b.Show.Theater.Id,
                        Name = b.Show.Theater.Name
                    }
                }
            })
            .ToListAsync(cancellationToken);
        }

        public Task<Booking?> GetBookingByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _dbContext.Bookings
            .Select(b => new Booking
            {
                Id = b.Id,
                UserId = b.UserId,
                ShowId = b.ShowId,
                SeatRow = b.SeatRow,
                SeatNumber = b.SeatNumber,
                Price = b.Price,
                Status = b.Status,
                BookingDateTime = b.BookingDateTime,
                User = new User
                {
                    Id = b.User.Id,
                    Email = b.User.Email,
                    UserName = b.User.UserName
                },
                Show = new Show
                {
                    Id = b.Show.Id,
                    Date = b.Show.Date,
                    StartTime = b.Show.StartTime,
                    EndTime = b.Show.EndTime,
                    Movie = new Movie
                    {
                        Id = b.Show.Movie.Id,
                        Title = b.Show.Movie.Title
                    },
                    Theater = new Theater
                    {
                        Id = b.Show.Theater.Id,
                        Name = b.Show.Theater.Name
                    }
                }
            })
            .SingleOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public Task<int> CreateBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> DeleteBookingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> UpdateBookingAsync(Booking booking)
        {
            throw new NotImplementedException();
        }
   }
}
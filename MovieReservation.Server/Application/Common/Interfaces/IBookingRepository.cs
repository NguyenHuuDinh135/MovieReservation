using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReservation.Server.Application.Features.Bookings.Commands.UpdateBooking;

namespace MovieReservation.Server.Application.Common.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken);
        Task<Booking> GetBookingByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateBookingAsync(Booking booking, CancellationToken cancellationToken);
        Task<Unit> UpdateBookingAsync(Booking booking, CancellationToken cancellationToken);
        Task<Unit> DeleteBookingAsync(Booking booking, CancellationToken cancellationToken);
    }
}
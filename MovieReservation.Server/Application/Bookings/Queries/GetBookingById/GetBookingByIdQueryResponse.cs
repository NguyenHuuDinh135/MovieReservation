using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int ShowId { get; set; }
        public string SeatRow { get; set; } = null!;
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
        public DateTime BookingDateTime { get; set; }
        public string UserEmail { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowStartTime { get; set; }
        public TimeSpan ShowEndTime { get; set; }
        public string MovieTitle { get; set; } = null!;
        public string TheaterName { get; set; } = null!;
    }
}
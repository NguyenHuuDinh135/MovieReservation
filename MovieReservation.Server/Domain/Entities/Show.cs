using System.Collections.Generic;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Domain.Entities
{
    public class Show
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public ShowStatus Status { get; set; }
        public ShowType Type { get; set; }

        public Movie Movie { get; set; }
        public Theater Theater { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
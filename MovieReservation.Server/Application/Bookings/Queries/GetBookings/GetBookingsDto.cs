using AutoMapper;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookings
{
    public class GetBookingsDto
    {
        public int Id { get; init; }
        public string UserId { get; init; }
        public int ShowId { get; init; }
        public string SeatRow { get; init; }
        public int SeatNumber { get; init; }
        public float Price { get; init; }
        public BookingStatus Status { get; init; }
        public DateTime BookingDateTime { get; init; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Booking, GetBookingsDto>();
            }
        }
    }
}
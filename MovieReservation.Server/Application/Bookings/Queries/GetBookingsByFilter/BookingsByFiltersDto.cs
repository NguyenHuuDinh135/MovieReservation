using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByFilter
{
    public class BookingsByFiltersDto
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
                CreateMap<Booking, BookingsByFiltersDto>();
            }
        }
    }
}
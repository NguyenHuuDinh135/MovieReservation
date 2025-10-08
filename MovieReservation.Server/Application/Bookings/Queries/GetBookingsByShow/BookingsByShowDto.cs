using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByShow
{
    public class BookingsByShowDto
    {
        public string SeatRow { get; init; }
        public int SeatNumber { get; init; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Booking, BookingsByShowDto>();
            }
        }
    }
}
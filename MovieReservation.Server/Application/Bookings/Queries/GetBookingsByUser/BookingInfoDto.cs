using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByUser
{
    public class BookingInfoDto
    {
        public int BookingId { get; set; }
        public float Price { get; set; }
        public string SeatRow { get; set; }
        public int SeatNumber { get; set; }
        public string BookingStatus { get; set; }
        public DateTime BookingDateTime { get; set; }

         public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Booking, BookingInfoDto>();
            }
        }
    }
}
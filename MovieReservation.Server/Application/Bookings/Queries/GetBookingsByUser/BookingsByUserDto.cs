using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByUser
{
    public class BookingsByUserDto
    {
        public string Title { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public ShowInfoDto Show { get; set; } = new();
        public List<BookingInfoDto> Bookings { get; set; } = new();

       
        
    }
}
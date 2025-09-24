using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieReservation.Server.Application.Bookings.Queries.GetBookingById;
using MovieReservation.Server.Application.Bookings.Queries.GetBookings;
using MovieReservation.Server.Application.Features.Bookings.Commands.CreateBooking;

namespace MovieReservation.Server.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Booking, GetBookingsQueryResponse>();
            CreateMap<Booking, GetBookingByIdQueryResponse>();
            CreateMap<CreateBookingCommand, Booking>();
        }
    }
}
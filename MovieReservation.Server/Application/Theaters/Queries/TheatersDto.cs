using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Theaters.Queries.GetTheaters
{
    public class TheatersDto
    {
        private int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumOfRows { get; set; }
        public int SeatsPerRow { get; set; }
        public TheaterType Type { get; set; }
        public List<TheaterSeatDto> Missing { get; set; } = [];
        public List<TheaterSeatDto> Blocked { get; set; } = [];
    }
    
    public class TheaterSeatDto
    {
        public string SeatRow { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
    }

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Theater, TheatersDto>()
                .ForMember(dest => dest.Missing, opt => opt.MapFrom(src => src.TheaterSeats.Where(seat => seat.Type == SeatType.Missing)))
                .ForMember(dest => dest.Blocked, opt => opt.MapFrom(src => src.TheaterSeats.Where(seat => seat.Type == SeatType.Blocked)));

            CreateMap<TheaterSeat, TheaterSeatDto>();
        }
    }
}
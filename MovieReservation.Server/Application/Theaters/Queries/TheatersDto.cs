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
        public TheaterType Theater_Type { get; set; }
    }

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Theater, TheatersDto>();
        }
    }
}
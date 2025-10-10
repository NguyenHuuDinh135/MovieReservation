using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Shows.Queries
{
    public class ShowsDto
    {
        public int Id { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public DateTime Date { get; init; }
        public int MovieId { get; init; }
        public int TheaterId { get; init; }
        public ShowStatus Status { get; init; }
        public ShowType Type { get; init; }
        
        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Show, ShowsDto>();
            }
        }
    }
}
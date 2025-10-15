using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Genres.Queries.GetGenresById
{
    public class GetGenresById
    {
        public int Id { get; init; }
        public string Name { gett; init; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Genre, GetGenresByIdDto>();
            }
        }
    }
}
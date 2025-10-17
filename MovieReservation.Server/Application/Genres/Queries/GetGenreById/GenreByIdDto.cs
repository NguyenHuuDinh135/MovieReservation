using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReservation.Server.Domain.Entities;
using AutoMapper;

namespace MovieReservation.Server.Application.Genres.Queries.GetGenreById
{
    public class GenreByIdDto
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Genre, GenreByIdDto>();
            }
        }
    }
}
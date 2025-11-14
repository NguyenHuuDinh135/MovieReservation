using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Genres.Queries.GetGenreByMovie
{
    public class GenresByMovieDto
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Genre, GenresByMovieDto>();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Movies.Queries.GetFilteredMovies
{
    public class FilteredMoviesDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal? Rating { get; set; }
        public string TrailerUrl { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public MovieType MovieType { get; set; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Movie, FilteredMoviesDto>();
            }
        }
    }
}
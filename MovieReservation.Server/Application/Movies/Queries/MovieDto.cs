using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Movies.Queries
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal? Rating { get; set; }
        public string TrailerUrl { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public MovieType MovieType { get; set; }
        public List<GenresDto> Genres { get; set; } = [];

    }
    public class GenresDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    
    public class Mapping : Profile
    {
        public Mapping()
        {
            // Mapping Movie → MoviesDto
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MovieGenres));

            // Mapping MovieGenre → GenresDto
            CreateMap<MovieGenre, GenresDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Genre.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Genre.Name));
        }
    }

}
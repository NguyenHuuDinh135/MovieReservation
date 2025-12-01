using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Movies.Queries.GetPersonForMovie
{
    public class PersonsForMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<PersonDto> Persons { get; set; } = new();
    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public byte Age { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
    }
    
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Movie, PersonsForMovieDto>()
                .ForMember(dest => dest.Persons, ops => ops.MapFrom(src => src.MoviePersons.Select(mr => mr.Person)));
            
            CreateMap<Person, PersonDto>();
        }
    }
}
using AutoMapper;

namespace MovieReservation.Server.Application.Persons.Queries.GetPersonByMovie
{
    public class PersonByMovieDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public byte Age { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public RoleType RoleType { get; set; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<MoviePerson, PersonByMovieDto>()
                    .ForMember(dest => dest.Id, ops => ops.MapFrom(src => src.Person.Id))
                    .ForMember(dest => dest.FullName, ops => ops.MapFrom(src => src.Person.FullName))
                    .ForMember(dest => dest.Age, ops => ops.MapFrom(src => src.Person.Age))
                    .ForMember(dest => dest.PictureUrl, ops => ops.MapFrom(src => src.Person.PictureUrl))
                    .ForMember(dest => dest.RoleType, ops => ops.MapFrom(src => src.RoleType));
            }
        }
    }
}

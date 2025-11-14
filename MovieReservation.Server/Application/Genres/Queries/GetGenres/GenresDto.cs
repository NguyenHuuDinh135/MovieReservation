using AutoMapper;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Genres.Queries.GetGenres
{
    public class GenresDto
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Genre, GenresDto>();
            }
        }
    }
}
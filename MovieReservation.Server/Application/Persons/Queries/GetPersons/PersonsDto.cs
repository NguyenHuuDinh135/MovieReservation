using AutoMapper;

namespace MovieReservation.Server.Application.Persons.Queries.GetPersons
{
    public class PersonsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public byte Age { get; set; }
        public string PictureUrl { get; set; } = string.Empty;

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Person, PersonsDto>();
            }
        }
    }
}

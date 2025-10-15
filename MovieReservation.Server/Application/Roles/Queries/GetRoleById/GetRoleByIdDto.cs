using AutoMapper;
using MovieReservation.Server.Domain.Entities;

namespace MovieReservation.Server.Application.Roles.Queries.GetRoleById
{
    public class RoleByIdDto
    {
        public int Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public int Age { get; init; }
        public string? PictureUrl { get; init; }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Role, RoleByIdDto>();
            }
        }
    }
}

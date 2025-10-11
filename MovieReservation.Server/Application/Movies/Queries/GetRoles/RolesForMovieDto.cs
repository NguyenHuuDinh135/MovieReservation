using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieReservation.Server.Application.Movies.Queries.GetRolesForMovie
{
    public class RolesForMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<RoleDto> Roles { get; set; } = new();
    }

    public class RoleDto
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
            CreateMap<Movie, RolesForMovieDto>()
                .ForMember(dest => dest.Roles, ops => ops.MapFrom(src => src.MovieRoles.Select(mr => mr.Role)));
            
            CreateMap<Role, RoleDto>();
        }
    }
}
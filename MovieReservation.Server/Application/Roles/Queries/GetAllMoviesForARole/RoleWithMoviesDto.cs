using System.Collections.Generic;

namespace MovieReservation.Server.Application.Roles.Queries.GetAllMoviesForARole
{
    public class RoleWithMoviesDto
    {
        public int RoleId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PictureUrl { get; set; } = string.Empty;

        public List<MovieForRoleDto> Movies { get; set; } = new();
    }
}

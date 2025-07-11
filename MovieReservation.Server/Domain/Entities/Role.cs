using System.Collections.Generic;

namespace MovieReservation.Server.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public byte Age { get; set; }
        public string PictureUrl { get; set; } = string.Empty;

        public ICollection<MovieRole> MovieRoles { get; set; } = new List<MovieRole>();
    }
}
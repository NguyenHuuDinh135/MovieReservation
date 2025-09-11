using MovieReservation.Server.Domain.Enums;
namespace MovieReservation.Server.Domain.Entities
{
    

    public class MovieRole 
    {
        public int MovieId { get; set; }
        public int RoleId { get; set; }
        public RoleType RoleType { get; set; }

        public Movie Movie { get; set; }
        public Role Role { get; set; }
    }
}
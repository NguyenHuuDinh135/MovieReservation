using MovieReservation.Server.Domain.Enums;
namespace MovieReservation.Server.Domain.Entities
{
    public class MoviePerson 
    {
        public int MovieId { get; set; }
        public int PersonId { get; set; }
        public RoleType RoleType { get; set; }

        public Movie Movie { get; set; }
        public Person Person { get; set; }
    }
}

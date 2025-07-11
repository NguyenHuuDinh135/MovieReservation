using System.Collections.Generic;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Domain.Entities
{
    public class Theater
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumOfRows { get; set; }
        public int SeatsPerRow { get; set; }
        public TheaterType Type { get; set; }

        public ICollection<Show> Shows { get; set; } = new List<Show>();
        public ICollection<TheaterSeat> TheaterSeats { get; set; } = new List<TheaterSeat>();
    }
}
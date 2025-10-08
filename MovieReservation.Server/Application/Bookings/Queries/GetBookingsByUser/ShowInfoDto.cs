using AutoMapper;
namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByUser
{
   public class ShowInfoDto
   {
      public int ShowId { get; set; }
      public string ShowType { get; set; } = string.Empty; // hoặc enum nếu muốn
      public DateTime ShowDatetime { get; set; } // Date + StartTime
       public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Booking, ShowInfoDto>();
            }
        }
   }
}

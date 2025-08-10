using MovieReservation.Server.Domain.Enums;
namespace MovieReservation.Server.Domain.Entities
{
    

    public class Payment
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public PaymentMethod Method { get; set; }
        public string UserId { get; set; }
        public int ShowId { get; set; }

        public User User { get; set; }
        public Show Show { get; set; }
    }
}
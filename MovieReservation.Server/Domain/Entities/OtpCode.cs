namespace MovieReservation.Server.Domain.Entities
{
    public class OtpCode
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string OTP { get; set; } = string.Empty;
        public DateTime ExpirationDateTime { get; set; }

        public User User { get; set; }
    }
}
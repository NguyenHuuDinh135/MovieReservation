using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Domain.Entities
{
    public class User : IdentityUser
    {

        public string Address { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
       // public UserRole Role { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        //public OtpCode? OtpCode { get; set; }
    }
}

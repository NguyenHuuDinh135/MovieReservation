using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommand : IRequest
    {
        public int Id { get; set; }
    }
}
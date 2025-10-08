using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReservation.Server.Application.Bookings.Commands.UpdateBooking;

namespace MovieReservation.Server.Application.Bookings.Commands.UpdateBooking
{
    public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
    {
        public UpdateBookingCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.SeatRow).NotEmpty();
            RuleFor(x => x.SeatNumber).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
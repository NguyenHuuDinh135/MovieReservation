using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        // Xử lý validate cho CreateBookingCommand
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.ShowId)
                .GreaterThan(0).WithMessage("ShowId must be greater than 0.");

            RuleFor(x => x.SeatRow)
                .NotEmpty().WithMessage("SeatRow is required.")
                .MaximumLength(5).WithMessage("SeatRow cannot exceed 5 characters.");

            RuleFor(x => x.SeatNumber)
                .GreaterThan(0).WithMessage("SeatNumber must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid booking status.");

            RuleFor(x => x.BookingDateTime)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("BookingDateTime cannot be in the future.");
        }
    }
}
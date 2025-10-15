using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Genres.Commands.CreateBooking
{
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        // Xử lý validate cho GenreBookingCommand
        public CreateGenreCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
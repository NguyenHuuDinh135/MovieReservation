using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReservation.Server.Application.Bookings.Commands.UpdateBooking;
namespace MovieReservation.Server.Application.Genres.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
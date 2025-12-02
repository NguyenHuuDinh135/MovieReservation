namespace MovieReservation.Server.Application.Persons.Commands.RemovePersonFromMovie
{
    public class RemovePersonFromMovieCommandValidator : AbstractValidator<RemovePersonFromMovieCommand>
    {
        public RemovePersonFromMovieCommandValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("MovieId must be greater than 0.");

            RuleFor(x => x.PersonId)
                .GreaterThan(0).WithMessage("PersonId must be greater than 0.");
        }
    }
}

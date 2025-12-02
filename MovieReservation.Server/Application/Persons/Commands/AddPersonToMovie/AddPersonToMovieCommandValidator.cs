namespace MovieReservation.Server.Application.Persons.Commands.AddPersonToMovie
{
    public class AddPersonToMovieCommandValidator : AbstractValidator<AddPersonToMovieCommand>
    {
        public AddPersonToMovieCommandValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("MovieId must be greater than 0.");

            RuleFor(x => x.PersonId)
                .GreaterThan(0).WithMessage("PersonId must be greater than 0.");

            RuleFor(x => x.RoleType)
                .IsInEnum().WithMessage("RoleType must be a valid RoleType enum value.");
        }
    }
}

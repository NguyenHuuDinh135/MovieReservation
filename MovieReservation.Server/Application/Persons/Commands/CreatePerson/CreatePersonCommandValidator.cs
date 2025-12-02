namespace MovieReservation.Server.Application.Persons.Commands.CreatePerson
{
    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(200).WithMessage("FullName must not exceed 200 characters.");

            RuleFor(x => x.Age)
                .GreaterThan((byte)0).WithMessage("Age must be greater than 0.")
                .LessThanOrEqualTo((byte)120).WithMessage("Age must be less than or equal to 120.");

            RuleFor(x => x.PictureUrl)
                .MaximumLength(500).WithMessage("PictureUrl must not exceed 500 characters.");
        }
    }
}

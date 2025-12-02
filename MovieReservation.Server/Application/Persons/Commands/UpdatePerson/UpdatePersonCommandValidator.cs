namespace MovieReservation.Server.Application.Persons.Commands.UpdatePerson
{
    public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

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

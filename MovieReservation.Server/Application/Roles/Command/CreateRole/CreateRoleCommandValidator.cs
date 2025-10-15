using FluentValidation;

namespace MovieReservation.Server.Application.Roles.Command.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100);
            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0");
        }
    }
}

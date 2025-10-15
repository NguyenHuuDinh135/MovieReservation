using FluentValidation;

namespace MovieReservation.Server.Application.Roles.Command.UpdateRole
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Age).GreaterThan(0);
        }
    }
}

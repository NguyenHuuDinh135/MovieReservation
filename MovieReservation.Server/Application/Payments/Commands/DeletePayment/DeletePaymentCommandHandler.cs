using MediatR;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;

namespace MovieReservation.Server.Application.Payments.Commands.DeletePayment;

public record DeletePaymentCommand : IRequest<Unit>
{
    public int Id { get; init; }
}

public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, Unit>
{
    private readonly IMovieReservationDbContext _context;

    public DeletePaymentCommandHandler(IMovieReservationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FindAsync(new object[] { request.Id }, cancellationToken);
        if (payment == null)
            throw new NotFoundException("Payment not found.");

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
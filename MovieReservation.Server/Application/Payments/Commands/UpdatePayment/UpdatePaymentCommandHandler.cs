using MediatR;
using MovieReservation.Server.Application.Common.Exceptions;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Payments.Commands.UpdatePayment;

public record UpdatePaymentCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Amount { get; init; }
    public DateTime PaymentDateTime { get; init; } = DateTime.Now;
    public PaymentMethod Method { get; init; } = PaymentMethod.Card;
    public string UserId { get; init; } = string.Empty;
    public int ShowId { get; init; }
}

public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Unit>
{
    private readonly IMovieReservationDbContext _context;

    public UpdatePaymentCommandHandler(IMovieReservationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FindAsync(new object[] { request.Id }, cancellationToken);
        if (payment == null)
            throw new NotFoundException("Payment not found.");

        // update fields
        payment.Amount = request.Amount;
        payment.PaymentDateTime = request.PaymentDateTime;
        payment.Method = request.Method;
        payment.UserId = request.UserId;
        payment.ShowId = request.ShowId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
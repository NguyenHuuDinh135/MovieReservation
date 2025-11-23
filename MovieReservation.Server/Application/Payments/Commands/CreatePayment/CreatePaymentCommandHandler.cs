using MediatR;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Payments.Commands.CreatePayment;

public record CreatePaymentCommand : IRequest<int>
{
    public int Amount { get; init; }
    public DateTime PaymentDateTime { get; init; } = DateTime.Now;
    public PaymentMethod Method { get; init; } = PaymentMethod.Card;
    public string UserId { get; init; } = string.Empty;
    public int ShowId { get; init; }
}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, int>
{
    private readonly IMovieReservationDbContext _context;

    public CreatePaymentCommandHandler(IMovieReservationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            Amount = request.Amount,
            PaymentDateTime = request.PaymentDateTime,
            Method = request.Method,
            UserId = request.UserId,
            ShowId = request.ShowId
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }
}

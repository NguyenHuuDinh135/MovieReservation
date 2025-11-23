using MediatR;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Application.Payments.Queries.GetPayments;

public record GetPaymentsQuery : IRequest<List<PaymentListDto>>;

public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, List<PaymentListDto>>
{
    private readonly IMovieReservationDbContext _context;

    public GetPaymentsQueryHandler(IMovieReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentListDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Payments
            .AsNoTracking()
            .Include(p => p.Show)
                .ThenInclude(s => s.Movie)
            .Select(p => new PaymentListDto
            {
                PaymentId = p.Id,
                Amount = p.Amount,
                PaymentDateTime = p.PaymentDateTime,
                PaymentMethod = p.Method.ToString(),
                UserId = p.UserId,
                ShowId = p.ShowId
            })
            .ToListAsync(cancellationToken);

        if (result == null || result.Count == 0)
            throw new NotFoundException("No payments found.");

        return result;
    }
}

public class PaymentListDto
{
    public int PaymentId { get; set; }
    public int Amount { get; set; }
    public DateTime PaymentDateTime { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int ShowId { get; set; }
}
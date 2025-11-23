using MediatR;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Application.Payments.Queries.GetPaymentById;

public record GetPaymentByIdQuery : IRequest<PaymentByIdDto>
{
    public int Id { get; init; }
}

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentByIdDto>
{
    private readonly IMovieReservationDbContext _context;

    public GetPaymentByIdQueryHandler(IMovieReservationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentByIdDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var p = await _context.Payments
            .AsNoTracking()
            .Include(x => x.Show)
                .ThenInclude(s => s.Movie)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (p == null)
            throw new NotFoundException("Payment not found.");

        return new PaymentByIdDto
        {
            PaymentId = p.Id,
            Amount = p.Amount,
            PaymentDateTime = p.PaymentDateTime,
            PaymentMethod = p.Method.ToString(),
            Movie = p.Show?.Movie is not null
                ? new PaymentMovieDto { Title = p.Show.Movie.Title, PosterUrl = p.Show.Movie.PosterUrl }
                : null
        };
    }
}

public class PaymentByIdDto
{
    public int PaymentId { get; set; }
    public int Amount { get; set; }
    public DateTime PaymentDateTime { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentMovieDto? Movie { get; set; }
}

public class PaymentMovieDto
{
    public string Title { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
}
using MediatR;
using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Application.Payments.Queries.GetPaymentsByUser;

public record GetPaymentsByUserQuery : IRequest<List<PaymentForUserDto>>
{
    public string Id { get; init; } = string.Empty;
}

public class GetPaymentsByUserQueryHandler : IRequestHandler<GetPaymentsByUserQuery, List<PaymentForUserDto>>
{
    private readonly IMovieReservationDbContext _context;

    public GetPaymentsByUserQueryHandler(IMovieReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentForUserDto>> Handle(GetPaymentsByUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Payments
            .AsNoTracking()
            .Where(p => p.UserId == request.Id)
            .Include(p => p.Show)
                .ThenInclude(s => s.Movie)
            .Select(p => new PaymentForUserDto
            {
                PaymentId = p.Id,
                Amount = p.Amount,
                PaymentDateTime = p.PaymentDateTime,
                PaymentMethod = p.Method.ToString(),
                Movie = (p.Show != null && p.Show.Movie != null)
                    ? new PaymentMovieDto { Title = p.Show.Movie.Title, PosterUrl = p.Show.Movie.PosterUrl }
                    : null
            })
            .ToListAsync(cancellationToken);

        if (result == null || result.Count == 0)
            throw new NotFoundException("No payments found for the user.");

        return result;
    }
}

public class PaymentForUserDto
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
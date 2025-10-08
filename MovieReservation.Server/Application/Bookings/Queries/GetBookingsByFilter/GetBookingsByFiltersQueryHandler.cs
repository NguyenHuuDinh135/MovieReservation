using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByFilter
{
    public record GetBookingsByFiltersQuery : IRequest<PaginatedList<BookingsByFiltersDto>>
    {
        string? Status { get; init; }
        string? Search { get; init; }
        int PageNumber { get; set; } = 1;
        int PageSize { get; set; } = 10;
    }
   public class GetBookingsByFiltersHandler : IRequestHandler<GetBookingsByFiltersQuery, PaginatedList<BookingsByFiltersDto>>
   {
        public Task<PaginatedList<BookingsByFiltersDto>> Handle(GetBookingsByFiltersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
   }
}
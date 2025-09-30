using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<GetBookingByIdQueryResponse>
    {
        public int Id { get; set; }
    }
}
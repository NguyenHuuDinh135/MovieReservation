using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Exceptions;

namespace MovieReservation.Server.Application.Bookings.Queries.GetBookingsByUser
{
    public record GetBooingsByUserQuery : IRequest<List<BookingsByUserDto>>
    {
        public String Id { get; init; }
    }
   public class GetBookingsByUserQueryHandler : IRequestHandler<GetBooingsByUserQuery, List<BookingsByUserDto>>
   {
        private IMovieReservationDbContext _context;
        private IMapper _mapper;

        public GetBookingsByUserQueryHandler(IMovieReservationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookingsByUserDto>> Handle(GetBooingsByUserQuery request, CancellationToken cancellationToken)
        {
            // Lấy dữ liệu Booking + Show + Movie
            var bookingData = await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Where(b => b.UserId == request.Id)
                .ToListAsync(cancellationToken);

            // Group by ShowId (mỗi show là 1 nhóm)
            var grouped = bookingData
                .GroupBy(b => new
                {
                    ShowId = b.Show.Id,
                    b.Show.Movie.Title,
                    b.Show.Movie.PosterUrl,
                    b.Show.Type,
                    b.Show.Date,
                    b.Show.StartTime
                })
                .Select(g => new BookingsByUserDto
                {
                    Title = g.Key.Title,
                    PosterUrl = g.Key.PosterUrl,
                    Show = new ShowInfoDto
                    {
                        ShowId = g.Key.ShowId,
                        ShowType = g.Key.Type.ToString(),
                        ShowDatetime = g.Key.Date.Date + g.Key.StartTime
                    },
                    Bookings = g.Select(b => new BookingInfoDto
                    {
                        BookingId = b.Id,
                        Price = b.Price,
                        SeatRow = b.SeatRow,
                        SeatNumber = b.SeatNumber,
                        BookingStatus = b.Status.ToString().ToLowerInvariant(),
                        BookingDateTime = b.BookingDateTime
                    }).ToList()
                })
                .ToList();

            return grouped;

        }
   }
}